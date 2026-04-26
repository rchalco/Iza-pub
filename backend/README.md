# Iza Pub - Backend API 🚀

Este es el repositorio del backend para el sistema **Iza Pub**, diseñado para gestionar operaciones de discotecas/pubs, incluyendo ventas, inventarios, seguridad y reportes.

La API está construida bajo una arquitectura de N-Capas (N-Tier) utilizando las tecnologías más modernas del ecosistema Microsoft.

## 🛠️ Stack Tecnológico

*   **Framework Core:** [.NET 10.0](https://dotnet.microsoft.com/)
*   **ORM:** [Entity Framework Core 9.0](https://docs.microsoft.com/en-us/ef/core/)
*   **Bases de Datos Soportadas:** SQL Server, PostgreSQL, MySQL (Configurado dinámicamente)
*   **Documentación de API:** Swagger (Swashbuckle)
*   **Reportes:** DevExpress Reporting Core
*   **Generación de QR:** QRCoder

## 🏗️ Arquitectura del Proyecto

La solución (`Iza.Solution.sln`) está organizada en múltiples capas para mantener la separación de responsabilidades:

### 1. Infraestructura (`1. Infraestructure`)
*   **`CoreAccesLayer`**: Capa de acceso a datos. Contiene los contextos de base de datos de EF Core y la implementación de acceso a los distintos motores SQL.
*   **`PlumbingProps`**: Capa transversal para propiedades utilitarias, configuraciones generales y helpers compartidos por toda la solución.

### 2. Capas de Negocio / Dominio (`2. Business.Layers`)
*   **`Iza.Core`**: Contiene la lógica de negocio principal, entidades de base de datos (`DBEntities`), y reglas de dominio relacionadas a la gestión del pub (inventario, mesas, pedidos).
*   **`Security.Core`**: Maneja la lógica de dominio específica para la autenticación, autorización y gestión de usuarios.

### 3. Servicios y APIs (`3. Services`)
*   **`Iza.Services`**: Aplicación Web API principal que expone los endpoints RESTful para la operativa del pub.
*   **`Security.API`**: Microservicio/API dedicada a la gestión de seguridad, tokens y autenticación.

### 4. Pruebas (`4. Test`)
*   **`Iza.Core.Test`**: Proyecto destinado a las pruebas unitarias y de integración de la lógica de negocio.

## ⚙️ Requisitos Previos

*   [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) o superior.
*   Servidor de base de datos (SQL Server, PostgreSQL, o MySQL).
*   Visual Studio 2022 (v17.8+ recomendado) o Visual Studio Code.

## 🚀 Instalación y Ejecución Local

1. **Clonar el repositorio y navegar a la carpeta del backend:**
   ```bash
   cd backend
   ```

2. **Restaurar las dependencias (Paquetes NuGet):**
   ```bash
   dotnet restore Iza.Solution.sln
   ```

3. **Configurar la Base de Datos:**
   Asegúrate de revisar el archivo `appsettings.json` o `appsettings.Development.json` dentro de los proyectos `Iza.Services` y `Security.API` para configurar tu cadena de conexión (Connection String).

4. **Aplicar Migraciones (Si aplica):**
   ```bash
   dotnet ef database update --project CoreAccesLayer --startup-project Iza.Services
   ```

5. **Ejecutar las APIs:**
   
   *Para correr el servicio principal:*
   ```bash
   dotnet run --project Iza.Services/Iza.Services.csproj
   ```
   
   *Para correr el servicio de seguridad:*
   ```bash
   dotnet run --project Security.API/Security.API.csproj
   ```

6. **Explorar la API (Swagger):**
   Una vez en ejecución, abre tu navegador e ingresa a la ruta generada seguida de `/swagger` (ej. `https://localhost:5001/swagger`) para ver y probar los endpoints interactivos.

## 🤝 Contribuciones

Si vas a agregar nuevas funcionalidades:
1. Mantén la lógica de negocio estrictamente en `Iza.Core`.
2. Las consultas e interacciones con la base de datos deben pasar por `CoreAccesLayer`.
3. Mantén los controladores en `Iza.Services` lo más limpios posibles, delegando la lógica pesada a las capas inferiores.
