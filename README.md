# MetasYProyectos

Aplicacion web ASP.NET Core MVC en .NET 10 para administrar la configuracion de conexion a una base de datos Oracle. El proyecto esta organizado con Clean Architecture y separa dominio, casos de uso, infraestructura y presentacion web.

## Estado actual

- Solucion en formato `.slnx`.
- Interfaz web MVC con autenticacion por cookie para el administrador.
- Flujo inicial para crear clave de administrador y codigo de recuperacion.
- Pantalla protegida para configurar conexion Oracle.
- Prueba de conexion Oracle desde la UI.
- Persistencia local cifrada para la configuracion de base de datos.
- Persistencia local de credenciales administrativas con hash PBKDF2 y salt.
- Proyecto de pruebas creado, aun sin pruebas reales.

## Stack

- .NET 10 (`net10.0`)
- ASP.NET Core MVC
- MediatR para CQRS
- FluentValidation
- Oracle.ManagedDataAccess.Core
- ASP.NET Core Data Protection

## Estructura

```text
MetasYProyectos.Domain/
  Entities/                 Entidades y validaciones de dominio
  Enums/                    VersionOracle, TipoConexion
  Exceptions/               Excepciones del dominio
  Interfaces/               Contratos del dominio

MetasYProyectos.Application/
  DTOs/                     DTOs de configuracion
  Interfaces/               Servicios usados por los casos de uso
  Mappings/                 Mapeos entre dominio y DTOs
  UseCases/Configuracion/   Commands y Queries con MediatR
  Validators/               Validaciones con FluentValidation

MetasYProyectos.Infrastructure/
  Oracle/                   Verificacion de conexion Oracle
  Persistence/              Repositorio de configuracion cifrada
  Services/                 Servicio de configuracion

MetasYProyectos.Web/
  Controllers/              Admin, Configuracion, Home, MetasProyectos
  Services/                 Autenticacion administrativa local
  ViewModels/               Modelos de vista MVC
  Views/                    Vistas Razor
  wwwroot/                  Recursos estaticos
  Config/                   Archivo local bd.config
  App_Data/                 Archivo local admin.json
  DataProtection-Keys/      Llaves de Data Protection

MetasYProyectos.Test/
  Proyecto de pruebas creado, sin cobertura implementada.
```

## Dependencias entre proyectos

```text
Domain
Application     -> Domain
Infrastructure  -> Application, Domain
Web             -> Application, Infrastructure
Test            -> Application, Domain
```

## Ejecucion local

Restaurar, compilar y ejecutar:

```powershell
dotnet restore
dotnet build
dotnet run --project MetasYProyectos.Web
```

Modo watch:

```powershell
dotnet watch run --project MetasYProyectos.Web
```

Pruebas:

```powershell
dotnet test
```

> Nota: el proyecto `MetasYProyectos.Test` existe, pero todavia no contiene pruebas funcionales.

## Flujo de uso

1. Abrir la aplicacion web.
2. Entrar a `/admin`.
3. Si no existe administrador, crear la clave inicial.
4. Guardar el codigo de recuperacion generado.
5. Iniciar sesion.
6. Ir a la pantalla de configuracion de base de datos.
7. Registrar servidor, puerto, servicio/SID/TNS, usuario, password, version de Oracle y tipo de conexion.
8. Probar la conexion o guardar la configuracion.

La ruta por defecto actual es:

```text
{controller=Home}/{action=Index}/{id?}
```

La pantalla de configuracion esta protegida con `[Authorize]`.

## Persistencia local

La configuracion de Oracle se guarda en:

```text
MetasYProyectos.Web/Config/bd.config
```

El contenido se serializa como JSON y se cifra con ASP.NET Core Data Protection. Las llaves se persisten en:

```text
MetasYProyectos.Web/DataProtection-Keys/
```

Las credenciales del administrador se guardan en:

```text
MetasYProyectos.Web/App_Data/admin.json
```

El password y el codigo de recuperacion se almacenan como hash PBKDF2 con salt.

## Configuracion Oracle

La cadena de conexion se construye desde los datos capturados en la UI, no desde `appsettings.json`. Los modos soportados por el dominio son:

- `ServiceName`
- `SID`
- `TNS`

La version de Oracle se modela con el enum `VersionOracle`.

## Seguridad y datos sensibles

- No se debe versionar `Config/bd.config`, `App_Data/admin.json` ni `DataProtection-Keys/`.
- La UI usa ocho caracteres bullet como mascara de password.
- Al guardar o probar conexion con la mascara, los handlers recuperan el password real ya almacenado.
- La sesion administrativa usa cookies con expiracion de 2 horas.

## Notas de arquitectura

- `ConfiguracionBD` usa constructor privado y fabrica estatica `Crear(...)` para centralizar validaciones de dominio.
- Los handlers capturan excepciones y devuelven objetos de resultado con estado y mensaje.
- `ConfiguracionRepository` esta registrado como singleton.
- `IOracleConnectionChecker` y `IConfiguracionService` estan registrados como scoped.
- El lenguaje del dominio y de la UI esta en espanol.

## Pendientes visibles

- Agregar pruebas unitarias para dominio, validadores y handlers.
- Revisar si `HomeController` y `MetasProyectosController` seran parte definitiva del flujo.
- Excluir archivos locales sensibles si todavia aparecen en el arbol de trabajo.
- Normalizar acentos/mensajes en algunos textos de UI y errores.
