# Wasla

Delivery Management System built using ASP.NET Core MVC and 3-Tier Architecture.

## Technologies

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* HTML, CSS, JavaScript
* 3-Tier Architecture

---

## Clone Repository

```bash
git clone https://github.com/YoussefAbdeen4/Wasla.git
```

---

## Open Solution

Open the solution file:

```text
Wasla.slnx
```

using Visual Studio.

---

## Restore Packages

From Visual Studio:

```
Build → Restore NuGet Packages
```

or run:

```bash
dotnet restore
```

---

## Update Connection String

Open:

```text
Wasla.PR/appsettings.json
```

Update the SQL Server connection string.

Example:

```json
"ConnectionStrings": {
    "DefaultConnection":
    "Server=.;Database=WaslaDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Apply Database Migrations

Open Package Manager Console and run:

```powershell
Update-Database
```

or:

```bash
dotnet ef database update --project Wasla.DAL --startup-project Wasla.PR
```

---

## Run The Project

Set:

```text
Wasla.PR
```

as Startup Project.

Then press:

```text
Ctrl + F5
```

or run:

```bash
dotnet run --project Wasla.PR
```

---

## Daily Workflow
```bash
git pull
git add .
git commit -m "Your message"
git push
```

## Project Structure

```text
Wasla.sln
│
├── Wasla.PR                         ← Presentation Layer
│   │
│   ├── Controllers
│   │   │
│   │   ├── Admin
│   │   ├── Agent
│   │   ├── Auth
│   │   ├── Company
│   │   ├── Merchant
│   │   └── HomeController.cs
│   │
│   ├── ViewModels
│   │
│   ├── Views
│   │   │
│   │   ├── Admin
│   │   ├── Agent
│   │   ├── Auth
│   │   ├── Company
│   │   ├── Home
│   │   ├── Merchant
│   │   │
│   │   └── Shared
│   │       ├── _Layout.cshtml
│   │       ├── _AdminLayout.cshtml
│   │       ├── _AgentLayout.cshtml
│   │       ├── _CompanyLayout.cshtml
│   │       ├── _MerchantLayout.cshtml
│   │       └── _AuthLayout.cshtml
│   │
│   ├── wwwroot
│   ├── appsettings.json
│   └── Program.cs
│
├── Wasla.BLL                        ← Business Logic Layer
│   │
│   └── Services
│       │
│       ├── Auth  
│       ├── Admin
│       ├── Company
│       ├── Merchant
│       ├── Agent
│
└── Wasla.DAL                        ← Data Access Layer
    │
    ├── Data
    │   └── AppDbContext.cs
    │
    ├── Models
    ├── Configurations
    ├── Constraints
    ├── Enums
    └── Migrations
```

---

## Team Members

* Youssef Abdeen
* Mohamed Shady
* Mostafa Bakri
* Omnia Salah
* Doaa Abdelnaser

```
```
