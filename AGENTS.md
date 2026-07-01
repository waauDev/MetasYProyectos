# MetasYProyectos — AGENTS.md

## Stack
- **.NET 10** (`net10.0`), ASP.NET Core MVC, C#
- **Clean Architecture**: `Domain` → `Application` → `Infrastructure` → `Web`
- **MediatR** (CQRS), **FluentValidation**, **Oracle.ManagedDataAccess.Core**
- **Solution**: `.slnx` format (new .NET XML solution)

## Project dependencies
- `Domain` — zero dependencies. Entities, Enums, Interfaces, Exceptions
- `Application` → `Domain`. MediatR, FluentValidation, DTOs, Mappers
- `Infrastructure` → `Application` + `Domain`. Oracle, DataProtection, file-based encrypted JSON persistence
- `Web` → `Application` + `Infrastructure`. MVC, single controller (`Configuracion`)
- `Test` → `Application` + `Domain`. **Empty** — no tests written yet

## Commands
```powershell
dotnet build
dotnet test                         # no real tests exist
dotnet run --project MetasYProyectos.Web
dotnet watch run --project MetasYProyectos.Web
```

## Architecture quirks
- **Default route**: `{controller=Configuracion}/{action=Index}/{id?}` — no `HomeController`
- **Repository is Singleton** — `ConfiguracionRepository` stores encrypted JSON at `Config/bd.config` using `IDataProtector`
- **DataProtection keys** persisted to `DataProtection-Keys/` folder (auto-created)
- **Password masking**: UI uses `"••••••••"` (8 bullets) as sentinel; handlers detect it and reload real password from store
- **Connection strings** built manually for Oracle (ServiceName/SID/TNS modes), not from config files
- **UserSecretsId**: `metas-y-proyectos-secrets`
- **All code in Spanish**: domain language, error messages, comments, enums, identifiers

## Conventions / gotchas
- `TreatWarningsAsErrors` is **disabled** (`false`)
- `LangVersion` set to `latest` in Domain project only
- `ConfiguracionBD` entity uses a `private` constructor + static factory `Crear()` with domain validation
- `ConfiguracionBDDto` is a mutable `record` (not `readonly record struct`)
- Exceptions are caught in handlers and returned as result objects, not thrown
- No `appsettings.*` database connection strings — all goes via the UI form
