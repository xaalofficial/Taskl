# ProjectTaskManager Full Stack Application

A full stack project management application leveraging **.NET 10 (C#) and PostgreSQL**. The app provides user authentication (JWT), project & task management, and a modern Web API.

## Table of Contents
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Running the Backend (.NET 10 API)](#running-the-backend-net-10-api)
- [Database Setup (PostgreSQL)](#database-setup-postgresql)

---

## Tech Stack
- **Backend:** .NET 10 Web API (C#), Entity Framework Core, JWT Auth
- **Database:** PostgreSQL

## Project Structure
```
backend/   # .NET 10 solution, divided into API, Application, Infrastructure, Domain
```

---

## Running the Backend (.NET 10 API)

1. **Requirements:**
   - [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) (or compatible)
   - PostgreSQL (running locally or remotely)

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

## Database Setup (PostgreSQL)

1. Make sure [PostgreSQL](https://www.postgresql.org/download/) is installed and running.
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

- Ensure PostgreSQL is running before starting the backend and that the connection details in `appsettings.json` match your DB setup.

- Verify you can access PostgreSQL using:
   ```sh
   psql --version
   ```

If `psql` is not recognized:
- PostgreSQL may be installed, but its bin directory is not added to the PATH environment variable.
- Locate the PostgreSQL bin folder, usually:
  C:\Program Files\PostgreSQL\<version>\bin
- Add this path to Environment Variables -> PATH
- Restart your terminal after updating PATH

- For CORS or auth errors, ensure the API is running at `http://localhost:5000`.
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

---

Thank you!
