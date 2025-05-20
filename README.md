# Aplicación de Gestión de Facturas - Prueba Técnica Full Stack

Esta aplicación web permite la gestión completa de facturas y sus detalles, incluyendo operaciones CRUD (Crear, Leer, Actualizar, Eliminar). Fue desarrollada como parte de una prueba técnica utilizando un backend en C# con .NET 8, Entity Framework Core y SQL Server, y un frontend en Angular (v16+).

## Características Implementadas

La solución implementa un ciclo completo de gestión de facturas:

- **CRUD Completo para Facturas y sus Detalles:**
    - Creación de nuevas facturas con múltiples ítems de detalle.
    - Lectura y visualización de una lista de todas las facturas existentes.
    - Visualización detallada de una factura individual, incluyendo todos sus ítems.
    - Actualización de facturas existentes y sus ítems de detalle.
    - Eliminación de facturas.
- **Backend Profesional y Robusto:**
    - Arquitectura orientada a la separación de responsabilidades (Domain, Application, Infrastructure).
    - Implementación del patrón **CQRS** (Command Query Responsibility Segregation) con la librería **MediatR** para una lógica de aplicación desacoplada y organizada.
    - Mapeo eficiente de objetos entre capas (DTOs y Entidades) utilizando **AutoMapper**.
    - Validación de datos de entrada robusta y declarativa mediante **FluentValidation**.
    - Uso del **Result Pattern** para el manejo explícito de resultados de operaciones y errores de negocio, promoviendo un código más predecible.
    - **Middleware global para manejo de excepciones** no controladas, asegurando respuestas de error consistentes y logging.
    - Persistencia de datos con **Entity Framework Core (Code-First)**, utilizando **migraciones** para gestionar la evolución del esquema de la base de datos SQL Server. 
    - Documentación interactiva de la API y capacidad de prueba mediante **Swagger/OpenAPI**. 
- **Frontend Interactivo con Angular:**
    - Desarrollado con **Angular (v16+)** y TypeScript. 
    - **Servicios de Angular** para encapsular la comunicación con la API backend. 
    - **Formularios Reactivos (Reactive Forms)** para la creación y edición de facturas, incluyendo la gestión dinámica de ítems de detalle (`FormArray`) y validaciones del lado del cliente. 
    - **Enrutamiento (Angular Router)** para la navegación entre las diferentes vistas de la aplicación (lista, creación, edición, detalle). 
    - Interfaz de usuario estilizada y responsiva utilizando **Bootstrap**.
- **Prácticas Generales de Desarrollo:**
    - Código fuente en **inglés**.
    - Gestión del código fuente con **Git**, siguiendo un flujo de trabajo basado en **Git Flow** (ramas `main`, `develop`, y ramas de característica). 

## Requisitos Previos para Ejecución Local

### Backend (.NET API)
- **.NET 8 SDK:** (Verificar la versión instalada con `dotnet --version`).
- **SQL Server:** Se recomienda SQL Server Developer Edition o Express Edition para el entorno local.
- **Visual Studio 2022** (recomendado) o un editor compatible con .NET como VS Code.

### Frontend (Angular)
- **Node.js:** Se recomienda una versión LTS reciente (ej. v18.x o v20.x). 
- **Angular CLI:** Versión 16 o superior, instalado globalmente.
- **Visual Studio Code** (recomendado).

## Configuración y Ejecución Local del Backend

1.  **Clonar el Repositorio:**
    ```bash
    git clone [https://github.com/diego-Ballesteros/GestionFacturasApp.git](https://github.com/diego-Ballesteros/GestionFacturasApp.git)
    cd GestionFacturasApp 
    ```
    

2.  **Navegar a la Carpeta del Backend:**
    ```bash
    cd Facturacion.API 
    ```
    

3.  **Configurar la Cadena de Conexión a SQL Server:**
    * Abrir el archivo `appsettings.Development.json`.
    * Modificar la sección `ConnectionStrings:DefaultConnection` para que apunte a tu instancia local de SQL Server. Se recomienda crear una base de datos vacía previamente (ej. `InvoicingTestDB`).
    
    * **Ejemplo para SQL Server LocalDB (común con Visual Studio):**
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InvoicingTestDB;Trusted_Connection=True;TrustServerCertificate=True;"
        }
        ```
    * **Nota:** `TrustServerCertificate=True` es útil para desarrollo local si no tienes un certificado SSL de confianza para SQL Server.

4.  **Aplicar Migraciones de la Base de Datos:**
    * Este paso creará el esquema de la base de datos (tablas, relaciones, etc.) basado en las entidades de EF Core.
    * **Opción A (Desde la Consola del Administrador de Paquetes en Visual Studio):**
        1.  Abrir la solución del backend en Visual Studio.
        2.  Seleccionar el proyecto `Facturacion.API` como proyecto predeterminado en la Consola del Administrador de Paquetes.
        3.  Ejecutar: `Update-Database`
    * **Opción B (Desde la terminal):**
        1.  Asegurarse de estar en la carpeta del proyecto backend (`Facturacion.API`).
        2.  Ejecutar: `dotnet ef database update`

5.  **Ejecutar el Backend:**
    * **Desde Visual Studio:** Presionar F5 o el botón de "Play" (asegurándose de que el perfil de lanzamiento correcto esté seleccionado, usualmente el que usa Kestrel y HTTPS).
    * **O desde la terminal** (en la carpeta del proyecto backend):
        ```bash
        dotnet run
        ```

6.  **Verificar la API:**
    * La API estará escuchando en la URL configurada en `Properties/launchSettings.json` (ej. `https://localhost:7243`).
    * Se puede acceder a la documentación y probar los endpoints a través de Swagger UI en `https://localhost:7243/swagger`.

## Configuración y Ejecución Local del Frontend

1.  **Navegar a la Carpeta del Frontend:**
    * Desde la raíz del repositorio (`GestionFacturasApp`), ejecutar:
        ```bash
        cd frontend-invoicing
        ```

2.  **Instalar Dependencias de Node.js:**
    * Si es la primera vez o si hay cambios en `package.json`:
        ```bash
        npm install
        ```

3.  **Verificar la Configuración de la API URL:**
    * El archivo de configuración para el entorno de desarrollo es `src/environments/environment.ts`.
    * Asegúrate de que la propiedad `apiUrl` apunte a la URL base de tu backend en ejecución:
        ```typescript
        export const environment = {
          production: false,
          apiUrl: 'https://localhost:7243/api' 
        };
        ```

4.  **Ejecutar el Servidor de Desarrollo de Angular:**
    ```bash
    ng serve -o
    ```

5.  **Acceder a la Aplicación:**
    * La aplicación se abrirá automáticamente en tu navegador en `http://localhost:4200/`.

## Puntos Adicionales

* **Estructura del Código Backend:** El backend sigue una estructura que facilita la separación de responsabilidades, con carpetas para `Domain` (entidades), `Application` (lógica de CQRS, DTOs, validadores), e `Infrastructure` (persistencia con EF Core).
* **Manejo de Errores:** Se implementó un middleware global para capturar excepciones no controladas y devolver respuestas estandarizadas. Adicionalmente, el Result Pattern se usa en los handlers de CQRS para manejar resultados de operaciones de forma explícita.

---
