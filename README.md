# Team Tasks Dashboard

Prueba técnica full stack para gestión de proyectos, tareas y desarrolladores.

## Estructura

    team-tasks-dashboard/
    ├── database/
    ├── src/
    │   ├── backend/
    │   │   ├── TeamTasks.Api/
    │   │   ├── TeamTasks.Application/
    │   │   ├── TeamTasks.Domain/
    │   │   ├── TeamTasks.Infrastructure/
    │   │   │   └── Sql/
    │   │   │       ├── Procedures/
    │   │   │       └── Queries/
    │   │   └── TeamTasks.Tests/
    │   └── frontend/
    │       ├── src/
    │       ├── angular.json
    │       ├── package.json
    │       └── proxy.conf.json
    ├── TeamTasks.slnx
    └── README.md

## Stack

- Backend: .NET 10, ASP.NET Core Web API, Entity Framework Core, SQL Server
- Frontend: Angular 21, TypeScript, RxJS, HttpClient, Standalone Components, SCSS

## Base de datos

Verificar el servidor local de SQL Server y ajustar la cadena de conexión en `src/backend/TeamTasks.Api`.

Al iniciar el backend:
- se aplican las migraciones
- se crea la base si no existe
- el seeder carga datos iniciales si la base está vacía

## Seeder

La solución incluye `ApplicationDbSeeder`, que inserta datos iniciales de:
- developers
- projects
- tasks

## Archivos SQL

Ubicación:
- `src/backend/TeamTasks.Infrastructure/Sql/Procedures`
- `src/backend/TeamTasks.Infrastructure/Sql/Queries`

Incluidos:
- `usp_CreateTask.sql`
- `Query_DeveloperDelayRiskPrediction.sql`

Estos archivos no son necesarios para el funcionamiento actual de la aplicación; se entregan porque la prueba técnica los solicita.

## Reutilizables frontend

- `data-table`: componente reutilizable de tabla
- `enum-label-pipe`: pipe reutilizable para mostrar enums en texto legible

## Funcionalidades

- Dashboard con 3 tablas: Developer Workload, Project Health y Developer Delay Risk
- Vista de tareas por proyecto con filtros, paginación y panel de detalle
- Formulario funcional de nueva tarea
- Frontend conectado a backend real, sin mocks

## Ejecutar backend

    cd src/backend/TeamTasks.Api
    dotnet restore
    dotnet run

## Ejecutar frontend

    cd src/frontend
    npm install
    npm start -- --proxy-config proxy.conf.json