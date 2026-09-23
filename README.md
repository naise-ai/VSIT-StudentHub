# StudentHub — Student Management System

A modern ASP.NET Core MVC student management system for academic projects.

## Stack
- ASP.NET Core MVC / .NET 10
- Entity Framework Core + SQLite
- Cookie authentication
- Responsive custom CSS

## Run in VS Code
1. Open this folder in VS Code.
2. Open Terminal → New Terminal.
3. Run `dotnet restore`.
4. Run `dotnet run`.
5. Open the localhost URL printed by the terminal.

## Demo login
Email: `naise.shekhar@vsit.edu.in`
Password: `Admin@123`

The database is created and seeded automatically on first run.

## Local HTTPS and PWA

Trust the local ASP.NET Core HTTPS certificate once:

```text
dotnet dev-certs https --trust
```

Run the app with `dotnet run` and open `https://localhost:5001`. The app includes an installable PWA manifest, service worker, and 192x192/512x512 icons.
