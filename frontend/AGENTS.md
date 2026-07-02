# AGENTS.md - Especificaciones del Proyecto Gamatek-Discoteca

## Descripción General

Gamatek-Discoteca es una aplicación híbrida multiplataforma que gestiona el funcionamiento de discotecas. Combina frontend web progresivo con contenedores nativos para Android, proporcionando una solución completa para el control de ventas, inventario, caja, tintorería y gestión de usuarios.

## Arquitectura del Proyecto

### Frontend
- **Framework**: Angular 18
- **UI**: Ionic 8
- **Compilación nativa**: Capacitor 8
- **Build**: Vite (mediante Angular CLI)
- **Base de datos local**: @ionic/storage-angular

### Backend
- **Framework**: .NET Core (C#)
- **Arquitectura**: Layers
  - CoreAccesLayer: Capa de acceso a datos
  - Iza.Core: Lógica de negocio
  - Iza.Services: Servicios de la aplicación
  - Security.API: API de seguridad
  - Security.Core: Lógica de seguridad

### Integración
- REST API entre frontend y backend
- Autenticación segura con tokens
- Base de datos SQL (detalles adicionales en backend)

## Funcionalidades Principales

### 1. Gestión de Ventas
- **Páginas**: venta, venta-express, venta-busqueda, pedido-mesa
- **Componentes**: busca-producto, lista-producto, datos-factura, forma-pago, reader-card, finger-capture
- **Funcionalidades**:
  - Procesamiento de ventas
  - Checkout rápido (express)
  - Búsqueda de productos
  - Atención de pedidos de mesa
  - Códigos de barras (impresión)

### 2. Inventario
- **Páginas**: inventario-general, inventario-final, apertura-inventario, cierre-inventario, asignacion-inventario, reposicion-producto, reposicion-producto2, movimiento-stock, entradas-salidas, asignacion-productos
- **Componentes**: custom-calendar, detalle-ingredientes
- **Funcionalidades**:
  - Inventario completo
  - Apertura y cierre de inventario
  - Asignación de productos a mesas/locales
  - Reposición automática de productos
  - Movimientos de stock
  - Cálculo de costos

### 3. Caja y Contabilidad
- **Páginas**: apertura-caja, cierre-caja, cierre-global, cierre-detalle-cajero, cierre-cajero, arqueo-cajero, cierre-operativo
- **Funcionalidades**:
  - Apertura y cierre de caja
  - Arqueo de cajeros
  - Cierre operativo diario
  - Reportes de caja global
  - Saldos por lugar de consumo

### 4. Tintorería
- **Páginas**: venta-tintoreria, tintoreria-entrega, reportes-tintoreria, reporte-caja
- **Funcionalidades**:
  - Gestión de prendas
  - Atención de clientes
  - Delivery/entrega de prendas
  - Reportes específicos de tintorería

### 5. Usuarios y Seguridad
- **Páginas**: login, cambio-contrasena, registro-huellas, verificacion-huella, abm-usuario, habilita-caja, personas, control-tarjetas
- **Servicios**: seguridad.service.ts, finger.service.ts, tarjeta.service.ts
- **Funcionalidades**:
  - Autenticación segura
  - Gestión de huellas dactilares
  - Registro y verificación biométrica
  - Control de acceso a cajas
  - Gestión de usuarios
  - Control de tarjetas

### 6. Control de Impresión
- **Página**: config-printer
- **Plugin**: capacitor-thermal-printer
- **Funcionalidades**:
  - Configuración de impresora Bluetooth
  - Impresión de tickets de venta
  - Códigos de barras
  - Configuración flexible de formatos

### 7. Reportes y Dashboard
- **Páginas**: reporte-tablero, reporte-ventas-consolidado, reporte-ventas-proceso, reportes-generales, dashboard-productos, reporte-cierre-global
- **Librerías**: Chart.js, DevExtreme
- **Funcionalidades**:
  - Dashboard con métricas principales
  - Reportes de ventas por proceso
  - Reportes consolidados
  - Reportes globales
  - Gráficos interactivos

### 8. Otros Componentes
- **Páginas**: home
- **Componentes**: custom-header, products-slides
- **Funcionalidades**: Navegación principal, slides de productos

## Estructura del Código

### Frontend Estructura
```
src/
├── app/
│   ├── app.component.ts          # Componente raíz
│   ├── app-routing.module.ts     # Enrutamiento principal
│   ├── components/               # Componentes reutilizables
│   │   ├── busca-producto/       # Búsqueda de productos
│   │   ├── cliente/              # Cliente
│   │   ├── custom-calendar/      # Calendario personalizado
│   │   ├── custom-camera/        # Cámara
│   │   ├── custom-header/        # Header personalizado
│   │   ├── datos-factura/        # Datos de factura
│   │   ├── detalle-ingredientes/ # Detalle de ingredientes
│   │   ├── finger-capture/       # Captura de huellas
│   │   ├── forma-pago/           # Formulario de pago
│   │   ├── lista-producto/       # Lista de productos
│   │   ├── menu/                 # Menú principal
│   │   ├── products-slides/      # Slides de productos
│   │   ├── reader-card/          # Lectura de tarjetas
│   │   └── registro-cliente-fac/ # Registro cliente factura
│   ├── guards/                   # Guards de seguridad
│   │   ├── sessionend.guard.ts   # Guarda al cerrar sesión
│   │   └── sessioninit.guard.ts  # Guarda al iniciar sesión
│   ├── helpers/                  # Utilidades
│   ├── interfaces/               # Interfaces TypeScript
│   │   ├── caja/                 # Interfaz caja
│   │   └── venta/                # Interfaz ventas
│   ├── pages/                    # Páginas principales (46+)
│   ├── pipes/                    # Pipes personalizados
│   └── services/                 # Servicios (12+)
│       ├── baseService.ts        # Servicio base
│       ├── DatabaseService.ts    # Servicio de base de datos local
│       ├── documento.service.ts  # Documentos
│       ├── finger.service.ts     # Huellas
│       ├── inventario.service.ts # Inventario
│       ├── persona.service.ts    # Personas
│       ├── seguridad.service.ts  # Seguridad
│       ├── stock.service.ts      # Stock
│       ├── tarjeta.service.ts    # Tarjetas
│       ├── tintoreria.service.ts # Tintorería
│       └── venta.service.ts      # Ventas
├── assets/                       # Recursos estáticos
├── environments/                 # Configuración por entorno
├── theme/                        # Variables globales
├── index.html                    # Plantilla principal
├── main.ts                       # Punto de entrada
└── polyfills.ts                  # Polyfills
```

### Backend Estructura
```
backend/
├── CoreAccesLayer/               # Capa de acceso a datos
├── Iza.Core/                     # Lógica de negocio
├── Iza.Core.Test/                # Tests unitarios
├── Iza.Services/                 # Servicios
├── Security.API/                 # API de seguridad
├── Security.Core/                # Lógica de seguridad
├── PlumbingProps/                # Propiedades del sistema
└── Iza.Solution.sln              # Solución principal
```

## Guías de Desarrollo

### Frontend Development

#### Inicio Rápido
```bash
npm install
npm start              # Modo desarrollo web
ionic serve            # Alternativa con Ionic
npm run build          # Build de desarrollo
ng build --configuration production  # Build de producción
```

#### Android Development
```bash
# Primeros pasos
npx cap add android
npm install capacitor-thermal-printer
npx cap sync android

# Flujo de trabajo
npm run build
npx cap sync android
npx cap open android

# Ejecutar en dispositivo/emulador
ionic capacitor run android
ionic capacitor run android --livereload --external
```

#### Testing
```bash
npm test                    # Tests unitarios
npm run test:watch          # Modo watch
npm run test:coverage       # Cobertura
ng lint                     # Linter
```

#### Plugins Nativos
El proyecto utiliza el plugin `capacitor-thermal-printer` para impresión Bluetooth. Este plugin debe instalarse explícitamente antes de sincronizar con Android:

```bash
npm install capacitor-thermal-printer
npx cap sync android
```

### Backend Development

El backend es un proyecto .NET Core con arquitectura en capas. Los archivos de compilación se generan automáticamente en la carpeta `.vscode/ide-messages.json` cuando se construye desde Visual Studio o desde otros IDEs.

## Estándares de Código

### Frontend
- **Lenguaje**: TypeScript 5.5.4
- **Framework**: Angular 18.2.0
- **UI Framework**: Ionic 8.4.0
- **Linter**: @angular-eslint
- **Formato**: Prettier (configuración en .eslintrc.json)

### Backend
- **Lenguaje**: C#
- **Framework**: .NET Core
- **Convenciones**: Estándar de Microsoft

## Patrones de Diseño

### Frontend
- **Páginas como componentes**: Cada página funciona como componente Angular
- **Lazy loading**: Todas las páginas se cargan dinámicamente
- **Guards**: Sesión iniciada/cerrada protege rutas
- **Services**: Pattern singleton para servicios
- **Interfaces**: TypeScript para contratos de datos

### Backend
- **Capas separadas**: Acceso a datos, lógica de negocio, servicios, API
- **Seguridad**: Autenticación y autorización basada en tokens
- **Inyección de dependencias**: Microsoft.Extensions.DependencyInjection

## Proceso de Desarrollo

1. **Configuración inicial**:
   ```bash
   npm install -g @ionic/cli @angular/cli @capacitor/cli
   ```

2. **Instalación de dependencias**:
   ```bash
   npm install
   ```

3. **Desarrollo**:
   - Modificar código en frontend/src/app/
   - Modificar código en backend/
   - Ejecutar `npm start` para desarrollo web
   - Ejecutar `npm run build` y `npx cap sync android` para Android

4. **Testing**:
   ```bash
   npm test
   npm run lint
   ```

5. **Android Deployment**:
   ```bash
   npm run build
   npx cap sync android
   npx cap open android
   ```

## Configuración de Entornos

### Development
- `src/environments/environment.ts`
- Servidor de desarrollo
- Logs detallados

### Production
- `src/environments/environment.prod.ts`
- Build optimizado
- Logs mínimos
- API de producción

## Archivos Especiales

### Capacitor
- `capacitor.config.json`: Configuración de Capacitor
- `ionic.config.json`: Configuración de Ionic
- `resources/`: Iconos y splash screens

### Plataforma Android
- `android/`: Proyecto nativo Android generado por Capacitor
- Build generado en `android/` después de sincronizar

## Problemas Comunes

### Plugin de impresora no aparece en Android
```bash
npm install capacitor-thermal-printer
npx cap sync android
```

### Android no refleja cambios
```bash
npm run build
npx cap sync android
```

### Algoritmo de impresión falla
1. Verificar impresión Bluetooth emparejada
2. Verificar configuración en `config-printer`
3. Reinstalar y sincronizar plugin: `npx cap sync android`

## Comandos Útiles

```bash
# Ver información del entorno Ionic
ionic info

# Ver dispositivos Android disponibles
ionic capacitor run android --list

# Generar componentes/páginas/servicios
ionic generate component components/nombre
ionic generate page pages/nombre-pagina
ionic generate service services/nombre-servicio

# Generar iconos y splash screens
npm install -D @capacitor/assets
npx capacitor-assets generate
```

## Integración Frontend-Backend

El frontend consume las APIs del backend mediante HTTP. Las implementaciones típicas incluyen:

- Autenticación: Login con JWT
- Manejo de sesiones: Token storage y validación
- API calls: HTTP services para CRUD operaciones
- Caching: LocalStorage para datos offline

## Notas Técnicas Importantes

1. **Plugin Capacitor Thermal Printer**: Debe estar en `node_modules` antes de la sincronización nativa
2. **Deploy Android**: Cada cambio en web requiere `npm run build` + `npx cap sync android`
3. **Testing**: Jest configurado para Angular 18
4. **Responsividad**: UI adaptada para dispositivos móviles y tablets
5. **Offline**: Datos almacenados localmente cuando es posible

## Patrones de Negocio

1. **Control de Inventario**: Asignación de productos a mesas, reposición automática
2. **Gestión de Caja**: Apertura, cierre y arqueo por cajero
3. **Tintorería**: Gestión completa del ciclo de vida de prendas
4. **Seguridad**: Verificación biométrica y control de acceso
5. **Impresión**: Tickets térmicos para códigos de barras

## Mantenimiento

- **Versiones**: Angular 18.2.0, Ionic 8.4.0, Capacitor 8
- **Dependencias actualizadas**: Últimas versiones compatibles
- **Testing**: 14 test files configurados
- **Build**: Compatible con producción

## Autor
@gamatek

## Licencia
Propietario privado

---

**Última actualización**: 2024-06-20
**Versión**: 1.0.0