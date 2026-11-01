# College CMS

A college management system for **Mtech College** that keeps departments, semesters, courses, students, staff and course enrollments in one web app.

Built with **ASP.NET Core 10 MVC**, **Entity Framework Core 10** and **SQLite**, using a classic layered architecture.

## Features

- **Dashboard**: live counts and a per-department breakdown of students, staff and courses
- **Students**: admission with auto-generated IDs (`STU-2026-0001`), personal and guardian details, and course enrollment
- **Employees**: staff records with department, position, hire and resignation dates
- **Courses, Departments, Levels**: full create/edit/delete with referential checks (a department that still has students can't be deleted)
- **Search, sortable columns and pagination** on every list
- **Authentication**: cookie sign-in, salted PBKDF2 password hashing, and role-based access (only admins can delete)
- **Security**: anti-forgery tokens on every form, POST-only deletes, server-side and client-side validation
- **Responsive UI**: Bootstrap 5 with light and dark themes
- **Tests**: unit tests for the business rules plus integration tests that boot the real app

## Architecture

```
CoreLayer          Entities (Student, Course, Department, ...) and shared paging types
DataAccessLayer    EF Core DbContext and one CRUD class per aggregate (search / sort / paging queries)
BusinessLayer      Services with business rules, password hashing, demo-data seeder, DI registration
PresentationLayer  ASP.NET Core MVC: controllers, view models, Razor views, sort-header tag helper
CollegeCMS.Tests   xUnit: service tests on in-memory SQLite + WebApplicationFactory integration tests
```

Each layer only references the one below it; the web project registers everything with `services.AddCollegeCms(connectionString)`.

## Getting started

No database server is needed: the SQLite database is created and filled with demo data on first run.

### Visual Studio 2026

1. Open `CollegeCMS.sln`. `PresentationLayer` is the startup project.
2. Press **F5** (or the green ▶ **http** button). The browser opens on the college home page.

Visual Studio 2022 cannot build .NET 10 projects; use Visual Studio 2026 (the free Community edition works) or the command line below.

### Command line / VS Code

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project PresentationLayer
```

Open http://localhost:5080. The home page describes the college; use **Sign in** at the top right and sign in with the demo administrator from `PresentationLayer/appsettings.json` (`DemoAdmin` section). Change those values before deploying anywhere public.

Run the tests:

```bash
dotnet test
```

To reset the demo data, stop the app and delete `PresentationLayer/App_Data/collegecms.db`.

## History

This project began in 2016 as an ASP.NET MVC 5 / Entity Framework 6 app on .NET Framework 4.5 backed by SQL Server. In 2026 it was migrated to ASP.NET Core 10, keeping the original four-layer design and domain model, and gaining:

- async EF Core data access with dependency injection instead of `new DataContext()` per call
- fixes for sorting on navigation properties, broken sort keys, student IDs that were never generated, and edits that dropped personal-info changes
- hashed passwords (previously stored in plain text) and role-based authorization
- a redesigned responsive UI, a dashboard, student enrollment management and an automated test suite
