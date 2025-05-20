# Aplicación de Gestión de Facturas - Prueba Técnica 

Esta aplicación permite gestionar facturas y sus detalles. Fue desarrollada como parte de una prueba técnica utilizando .NET 8, Entity Framework Core, SQL Server, y Angular 16+.

## Características Implementadas

- CRUD completo para Facturas y sus Detalles.
- Listado y visualización de detalles de facturas.
- Formulario reactivo para creación y edición de facturas.
- **Backend:**
    - Patrón CQRS con MediatR.
    - Mapeo de objetos con AutoMapper.
    - Validación con FluentValidation.
    - Result Pattern para manejo de resultados de operaciones.
    - Middleware global para manejo de errores.
    - Entity Framework Core Code-First con Migraciones.
    - Documentación de API con Swagger.
- **Frontend:**
    - Angular 16+.
    - Servicios para comunicación con API.
    - Reactive Forms con validaciones.
    - Enrutamiento con Angular Router.
    - Estilos con Bootstrap.
- **Otros:**
    - Control de versiones con Git y flujo Git Flow.

## Requisitos Previos

### Backend
- .NET 8 SDK
- SQL Server (Developer, Express, etc.)
- Visual Studio 2022 (recomendado)

### Frontend
- Node.js (v18.x o v20.x LTS recomendado)
- Angular CLI (v16+ globalmente)
- VS Code (recomendado)

## Configuración y Ejecución Local del Backend

1.  Clonar el repositorio: `git clone https://github.com/diego-Ballesteros/GestionFacturasApp.git`
2.  Navegar a la carpeta del backend: `cd GestionFacturasApp/Facturacion.API`
3.  **Configurar Cadena de Conexión:**
    - Abrir el archivo `appsettings.Development.json`.
    - Modificar la sección `ConnectionStrings:DefaultConnection` para apuntar a su instancia local de SQL Server y la base de datos deseada (se recomienda crear una base de datos vacía, ej. `InvoicingTestDB`).
    - Ejemplo:
      ```json
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=InvoicingTestDB;Trusted_Connection=True;TrustServerCertificate=True;"
      }
      ```
4.  **Aplicar Migraciones:**
    - Abrir la Consola del Administrador de Paquetes en Visual Studio (con `Facturacion.API` como proyecto predeterminado) y ejecutar:
      ```powershell
      Update-Database
      ```
    - O desde la terminal en la carpeta del proyecto API:
      ```bash
      dotnet ef database update
      ```
5.  **Ejecutar el Backend:**
    - Desde Visual Studio (presionar F5).
    - O desde la terminal:
      ```bash
      dotnet run
      ```
6.  La API estará disponible en las URLs especificadas (ej. `https://localhost:7243`) y Swagger en `https://localhost:7243/swagger`.

## Configuración y Ejecución Local del Frontend

1.  Navegar a la carpeta del frontend: `cd GestionFacturasApp/frontend-invoicing` 
2.  **Instalar Dependencias:**
    ```bash
    npm install
    ```
3.  **Verificar Configuración de API URL:**
    - El archivo `src/environments/environment.ts` contiene la `apiUrl`. Por defecto para desarrollo es:
      ```typescript
      apiUrl: 'https://localhost:7243/api' 
      ```
    - Asegúrate que coincida con la URL y puerto de tu backend en ejecución.
4.  **Ejecutar el Frontend:**
    ```bash
    ng serve -o 
    ```
5.  La aplicación se abrirá en `http://localhost:4200/`.

---
(Despliegue si lo implementas)