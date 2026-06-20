# Codegraph - Iza-pub

## Arquitectura General

```mermaid
graph TB
    subgraph Frontend["Frontend (Ionic/Angular 18)"]
        APP["AppComponent<br/>app.component.ts"]
        ROUTER["Router<br/>40+ lazy pages"]
        UI["Components<br/>14 shared components"]
        SVC["Services<br/>11 API services"]
        MODELS["Interfaces<br/>9 domains ~50 DTOs"]
        PWA["Capacitor 8<br/>Android/iOS/PWA"]
    end

    subgraph BackendAPI["Backend - API Layer"]
        IzaSvc["Iza.Services<br/>ASP.NET Core 10"]
        SecAPI["Security.API<br/>ASP.NET Core 10"]
        CTRL_V["APIVentaController<br/>22 endpoints"]
        CTRL_I["APIIventarioController<br/>13 endpoints"]
        CTRL_S["APISeguridadController<br/>3 endpoints"]
        CTRL_SEC["APISecurityController<br/>3 endpoints"]
    end

    subgraph Business["Backend - Business Layer"]
        IzaCore["Iza.Core"]
        SecCore["Security.Core"]
        ENG_V["EngineVentas<br/>Ventas/Caja"]
        ENG_I["EngineInventarios<br/>Inventarios"]
        ENG_S["EngineSeguridad<br/>Login/Menú"]
        ENG_SEC["EngineSecurity<br/>Login/Menú"]
        ENG_P["EngineImpresion<br/>Documentos"]
        ENG_B["EngineBackoffice"]
        DTOs["~55 DTOs<br/>6 dominios"]
        REPORTS["DevExpress Reports<br/>Voucher, CierreCaja, Asignación"]
    end

    subgraph DataAccess["Backend - Data Access Layer"]
        REPO["IRepository&lt;T&gt;<br/>Repository Pattern"]
        FACTORY["FactoryDataInterfaz<br/>Factory Pattern"]
        MSSQL["MSSQLRepository<br/>SQL Server + ADO.NET"]
        MYSQL["MySQLRepository<br/>MySQL EF Core"]
        EF["EF Core DbContext<br/>shFabula, seguridad, inventarioF"]
        SP["Stored Procedures<br/>SQL Server"]
    end

    subgraph Infrastructure["Backend - Infrastructure Layer"]
        CONF["ConfigManager<br/>appsettings.json"]
        LOG["NLog Logger<br/>Binnacle + Event"]
        WRAP["Response&lt;T&gt;<br/>Wrapper unificado"]
        DOC["Documentos<br/>Excel, Word, PDF, HTML"]
        UTIL["CrossUtil<br/>Mail, Serializer, Mapster"]
    end

    subgraph PinterUtil["PinterUtil - Windows Forms"]
        PT_MAIN["Printer.cs<br/>Form principal"]
        PT_TIMER["Timer 1s<br/>Polling"]
        PT_CLIENT["ClientHelper<br/>HTTP POST"]
        PT_DEVEX["DevExpress PDF<br/>Impresión"]
    end

    subgraph Database["SQL Server<br/>DBPubIZA"]
        DB_FABULA["shFabula<br/>Ventas/Pedidos"]
        DB_SEG["seguridad<br/>Usuarios/Roles"]
        DB_INV["inventarioF<br/>Inventarios"]
    end

    subgraph Ext["Servicios Externos"]
        SIN["SIN Bolivia<br/>Facturación Electrónica"]
        FINGER["Servicio Huella<br/>localhost:2525"]
        BT["Bluetooth Printer<br/>Capacitor Thermal"]
    end

    APP --> ROUTER
    APP --> UI
    ROUTER --> SVC
    SVC --> MODELS
    SVC --> CTRL_V
    SVC --> CTRL_I
    SVC --> CTRL_S
    SVC --> CTRL_SEC
    SVC --> Ext

    CTRL_V --> ENG_V
    CTRL_I --> ENG_I
    CTRL_S --> ENG_S
    CTRL_SEC --> ENG_SEC

    IzaCore --> ENG_V
    IzaCore --> ENG_I
    IzaCore --> ENG_S
    IzaCore --> ENG_P
    IzaCore --> ENG_B
    IzaCore --> DTOs
    IzaCore --> REPORTS
    SecCore --> ENG_SEC

    ENG_V --> REPO
    ENG_I --> REPO
    ENG_S --> REPO
    ENG_SEC --> REPO

    REPO --> FACTORY
    FACTORY --> MSSQL
    FACTORY --> MYSQL
    MSSQL --> SP
    MSSQL --> EF
    MYSQL --> EF
    SP --> Database
    EF --> Database

    IzaCore --> Infrastructure
    SecCore --> Infrastructure

    PT_TIMER --> PT_MAIN
    PT_CLIENT --> PT_MAIN
    PT_DEVEX --> PT_MAIN
    PT_MAIN --> CTRL_V
```

## Estructura de Proyectos

### Backend (.NET 10)

```
Iza.Solution.sln
├── PlumbingProps            # Infraestructura (utilidades cross-cutting)
│   ├── Config               # ConfigManager
│   ├── CrossUtil            # Extensiones, Mail, MapHelper, Serializer
│   ├── Document             # Excel, Word, PDF, HTML
│   ├── Exceptions           # ManagerException
│   ├── Logger               # NLog (Binnacle + Event)
│   ├── Services             # ClientHelper HTTP
│   └── Wrapper              # Response<T> unificado
│
├── CoreAccesLayer           # Capa de acceso a datos
│   ├── Interface            # IRepository<T>, FactoryDataInterfaz
│   ├── Implement/SQLServer  # MSSQLRepository (EF Core + ADO.NET)
│   ├── Implement/MySQL      # MySQLRepository
│   └── Wraper               # Entity<T> (stateEntity)
│
├── Iza.Core                 # Lógica de negocio principal
│   ├── Base                 # BaseManager (abstract)
│   ├── DBEntities           # DbContext + 33 entidades (shFabula)
│   ├── Domain               # ~55 DTOs (Venta, Inventario, Reportes, etc.)
│   ├── Engine               # EngineVentas, Inventarios, Seguridad, Impresion, Backoffice
│   └── Reports              # DevExpress Reports
│
├── Security.Core            # Lógica de seguridad
│   ├── Base                 # BaseManager (abstract)
│   ├── DBEntities           # DbContext + 37 entidades (3 schemas)
│   ├── Domain               # Login/Menú DTOs
│   └── Engine               # EngineSecurity
│
├── Iza.Services             # API principal (ASP.NET Core)
│   └── Services             # 4 Controllers (Venta, Inventario, Seguridad, Backoffice)
│
├── Security.API             # API de seguridad independiente
│   └── Services             # 1 Controller (Security)
│
└── Iza.Core.Test            # Tests NUnit
```

### Frontend (Ionic 8 / Angular 18)

```
frontend/
├── src/app/
│   ├── app.module.ts        # NgModule raíz
│   ├── app-routing.module   # 40+ rutas lazy-loaded
│   ├── app.component        # Componente raíz
│   │
│   ├── components/          # 14 componentes compartidos
│   │   ├── menu/            # Menú dinámico desde backend
│   │   ├── busca-producto/
│   │   ├── forma-pago/
│   │   ├── finger-capture/  # Captura de huella
│   │   ├── reader-card/     # Lector NFC/tarjeta
│   │   ├── products-slides/ # Carrusel de productos
│   │   ├── custom-header/
│   │   ├── custom-calendar/
│   │   ├── custom-camera/
│   │   ├── detalle-ingredientes/
│   │   ├── lista-producto/
│   │   ├── datos-factura/
│   │   ├── registro-cliente-fac/
│   │   └── cliente/
│   │
│   ├── pages/               # 43 páginas
│   │   ├── venta/           # Venta principal
│   │   ├── venta-express/
│   │   ├── pedido-mesa/     # Pedidos por mesa
│   │   ├── bandeja-pedidos/ # Bandeja cocina/barra
│   │   ├── apertura-caja/
│   │   ├── cierre-caja/
│   │   ├── cierre-cajero/
│   │   ├── cierre-global/
│   │   ├── inventario-general/
│   │   ├── inventario-final/
│   │   ├── asignacion-inventario/
│   │   ├── dashboard-productos/
│   │   ├── abm-usuario/
│   │   ├── login/
│   │   ├── config-printer/
│   │   ├── tintoreria/      # Módulo tintorería (4 páginas)
│   │   ├── registro-huellas/
│   │   ├── verificacion-huella/
│   │   ├── control-tarjetas/
│   │   └── ... (40+)
│   │
│   ├── services/            # 11 servicios API
│   │   ├── baseService.ts
│   │   ├── seguridad.service.ts
│   │   ├── venta.service.ts
│   │   ├── stock.service.ts
│   │   ├── inventario.service.ts
│   │   ├── persona.service.ts
│   │   ├── tarjeta.service.ts
│   │   ├── finger.service.ts
│   │   ├── tintoreria.service.ts
│   │   ├── fabula.service.ts
│   │   └── documento.service.ts
│   │
│   ├── interfaces/          # ~50 modelos TypeScript
│   │   ├── venta/
│   │   ├── inventario/
│   │   ├── general/
│   │   ├── caja/
│   │   ├── tarjeta/
│   │   ├── tintoreria/
│   │   ├── reportes/
│   │   ├── printer/
│   │   └── wraper/
│   │
│   └── guards/              # sessioninit, sessionend
│
└── capacitor.config.json    # Android/iOS
```

### PinterUtil (.NET Framework 4.8 - Windows Forms)

```
PrinterGamatek.sln
└── PrinterGamatek/
    ├── Program.cs           # Entry point
    ├── Printer.cs           # Form principal + lógica
    ├── CleintService/       # HTTP client
    │   ├── ClientHelper.cs
    │   ├── PrinterLineRequest.cs
    │   ├── PrinterLineResponse.cs
    │   └── ResponseQuery.cs
    └── App.config           # URL, idPrinter, namePrinter, timeFoWait
```

## Diagrama de Flujo de Datos

```mermaid
flowchart LR
    USER["Usuario<br/>Tablet/Móvil"] -->|Interacción| IONIC["App Ionic/Angular<br/>Frontend"]
    
    IONIC -->|HTTP POST JSON| API["Iza.Services<br/> :8034"]
    IONIC -->|HTTP POST JSON| SEC["Security.API<br/> :8034"]
    IONIC -->|HTTP REST| FINGER_SVC["Servicio Huella<br/>localhost:2525"]
    IONIC -->|Bluetooth| THERMAL["Impresora Térmica<br/>Bluetooth"]
    
    API -->|SP/EF| SQL[("SQL Server<br/>DBPubIZA")]
    SEC -->|SP/EF| SQL
    
    API -->|SOAP| SIN["SIN Bolivia<br/>Facturación"]
    
    PINTER["PinterUtil<br/>WinForms"] -->|HTTP Polling 1s| API
    PINTER -->|PDF| INKJET["Impresora Tinta<br/>Canon G3060"]
    
    API -->|Response&lt;T&gt;| IONIC
    SEC -->|Response&lt;T&gt;| IONIC
```

## Stack Tecnológico

| Capa | Tecnología | Versión |
|------|-----------|---------|
| **Backend** | .NET | 10.0 |
| **Backend** | ASP.NET Core | 10.0 |
| **Backend** | Entity Framework Core | 9.x |
| **Backend** | DevExpress Reporting | 23.1 |
| **Backend** | SQL Server | (principal) |
| **Backend** | MySQL | (soporte) |
| **Backend** | NLog | (logging) |
| **Backend** | iText7 / QRCoder | (PDF/QR) |
| **Frontend** | Angular | 18.2 |
| **Frontend** | Ionic | 8.4 |
| **Frontend** | Capacitor | 8 |
| **Frontend** | DevExtreme | 24.2 |
| **Frontend** | Chart.js | 3.7 |
| **Frontend** | Ionic Storage | (sesión) |
| **Desktop** | .NET Framework | 4.8 |
| **Desktop** | Windows Forms | (WinForms) |
| **Desktop** | DevExpress PDF | 23.1 |
| **Testing** | NUnit / Jest | |

## Dominios de Negocio

```mermaid
mindmap
  root((Iza-pub))
    Ventas
      Venta Rápida
      Venta por Mesa
      Pedidos
      Formas de Pago
      Anulación
      Reimpresión
    Caja
      Apertura
      Cierre
      Arqueo
      Movimientos
      Cierre Global
    Inventario
      Apertura
      Cierre
      Asignación
      Reposición
      Ingredientes/Recetas
    Seguridad
      Login
      Roles
      Menú Dinámico
      Usuarios
    Reportes
      Dashboard Productos
      Ventas por Día/Menú
      Cierre Consolidado
      Tablero
    Facturación
      SIN Bolivia
      Documentos Electrónicos
      Dosificación
    Tarjeta/Fidelización
      Lectura NFC
      Movimientos
      Clientes
    Biometría
      Registro Huella
      Verificación
    Tintorería
      Venta
      Entrega
      Reportes
    Impresión
      Voucher Térmico (Bluetooth)
      Documentos PDF (Tinta)
      Cola de Impresión
```

## Endpoints de API

### Iza.Services (`http://155.138.212.216:8034/api/`)

| Controller | Endpoints | Función |
|-----------|-----------|---------|
| **APIVenta** | 22 POST | Ventas, caja, formas de pago, anulación, reimpresión, documentos |
| **APIIventario** | 13 POST | Inventarios, asignaciones, dashboard, ingredientes |
| **APISeguridad** | 3 POST | Login, cambio contraseña, menú por rol |
| **APIBackoffice** | (stub) | - |

### Security.API (`http://155.138.212.216:8034/api/`)

| Controller | Endpoints | Función |
|-----------|-----------|---------|
| **APISecurity** | 3 POST | Login, cambio contraseña, menú por rol |

## Patrones Arquitectónicos

- **Layered Architecture**: 4 capas (Infrastructure → Business → API → Tests)
- **Repository Pattern**: `IRepository<TDbContext>` con implementaciones MSSQL/MySQL
- **Factory Pattern**: `FactoryDataInterfaz` crea repositorio según provider
- **Unit of Work**: Transacciones vía `Commit()`/`Rollback()` en repositorio
- **Response Wrapper**: `Response<T>` unificado con Estado (Success/Warning/Error/NoData)
- **Lazy Loading**: 40+ módulos Angular cargados bajo demanda
- **Guard Pattern**: `SessioninitGuard` / `SessionendGuard` para control de acceso