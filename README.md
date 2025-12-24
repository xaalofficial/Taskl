A full stack project management application leveraging **.NET 10 (C#), Angular 18, and PostgreSQL 16**. The app provides user authentication (JWT), project & task management, and a modern UI with TailwindCSS.

## Table of Contents
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Running the Backend (.NET 10 API)](#running-the-backend-net-10-api)
- [Running the Frontend (Angular)](#running-the-frontend-angular)
- [Database Setup (PostgreSQL)](#database-setup-postgresql)

---

## Tech Stack
- **Backend:** .NET 10 Web API (C#), Entity Framework Core, JWT Auth
- **Frontend:** Angular 18, TypeScript, TailwindCSS
- **Database:** PostgreSQL 16 (16.11)

## Project Structure
```
backend/   # .NET 10 solution, divided into API, Application, Infrastructure, Domain
frontend/  # Angular 18 + TailwindCSS UI
```

---

## Running the Backend (.NET 10 API)

1. **Requirements:**
   - [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) (or compatible)
   - PostgreSQL 16 (16.11 recommended) - running locally or remotely

2. **Setup:**
   ```sh
   # In backend/ProjectTaskManager.API
   cd backend/ProjectTaskManager.API

   # Restore dependencies
   dotnet restore

   # [Optional] Set/update DB connection in `appsettings.json`
   # Default (Postgres):
   # Host=localhost;Port=5432;Database=projecttaskmanager;Username=taskmanager;Password=admin

   # Run migrations and seed initial data
   dotnet run
   # The server will apply migrations & create a test user (email: test@example.com, pass: Test123!)
   ```

3. **Running the API:**
   ```sh
   dotnet run
   # API runs at http://localhost:5000
   # Swagger available at /swagger in Development
   ```

---

## Running the Frontend (Angular)

1. **Requirements:**
    - [Node.js (>=18)](https://nodejs.org/) & npm
    - [Angular CLI 18](https://angular.io/cli)

2. **Setup & Run:**
   ```sh
   cd frontend
   npm install
   npm start
   # App will run at http://localhost:4200
   ```
   - The Angular development server connects to the backend API at https://localhost:5000 (see `environment.ts` if needed).

---

## Database Setup (PostgreSQL)

1. Make sure [PostgreSQL 16](https://www.postgresql.org/download/) is installed and running.
2. Update `backend/ProjectTaskManager.API/appsettings.json` if you want to change DB settings:
   - Default:
     ```
     Host=localhost;Port=5432;Database=projecttaskmanager;Username=taskmanager;Password=admin
     ```
3. **Create the database and user (if not exists):**
   ```sql
   CREATE DATABASE projecttaskmanager;
   CREATE USER taskmanager WITH ENCRYPTED PASSWORD 'admin';
   GRANT ALL PRIVILEGES ON DATABASE projecttaskmanager TO taskmanager;
   ```
4. On first `dotnet run` **migrations are applied automatically** and a test user is seeded (see [DbInitializer](backend/ProjectTaskManager.Infrastructure/Persistence/DbInitializer.cs)).
   - Default seeded user:
      - email: `test@example.com`
      - password: `Test123!`

---

## Troubleshooting

- Ensure PostgreSQL 16 is running before starting the backend and that the connection details in `appsettings.json` match your DB setup.

- Verify you can access PostgreSQL using:
  ```sh
  psql --version
  ```
  If psql is not recognized:

  PostgreSQL may be installed, but its bin directory is not added to PATH.

  Locate the PostgreSQL bin folder, usually:

  `C:\Program Files\PostgreSQL\16\bin`

- For CORS or auth errors, ensure the Angular app is running at `http://localhost:4200` and API at `https://localhost:5000`.
- If you add new migrations, use:
  ```sh
  dotnet ef migrations add MigrationName --startup-project ProjectTaskManager.API
  dotnet ef database update --startup-project ProjectTaskManager.API
  ```

---

## Useful Scripts
- **Backend:**
  - `dotnet restore` – Restore NuGet packages
  - `dotnet run` – Start API & apply migrations
  - `dotnet ef database update` – Apply DB migration

- **Frontend:**
  - `npm start` / `ng serve` – Start Angular dev server
  - `npm run build` / `ng build` – Build UI

---

## Other
- **Swagger**: Enabled at `/swagger` when in Development environment
- **JWT Secret Key & Auth**: Configurable in `appsettings.json` (not for prod)
- **TailwindCSS**: See `frontend/tailwind.config.js` for customization

---

Thank you!