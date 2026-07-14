# MetasYProyectos

Aplicacion web ASP.NET Core MVC desarrollada en .NET 10 para gestionar la configuracion de conexion a una base de datos Oracle y la administracion inicial del acceso al sistema.

## Arquitectura

El proyecto sigue una **Arquitectura Limpia (Clean Architecture)** con cuatro capas principales:

```
Domain  →  Application  →  Infrastructure  →  Web
```

### Capas

- **MetasYProyectos.Domain**: entidades, enumeraciones, interfaces, excepciones y objetos de valor del dominio. Sin dependencias externas.
- **MetasYProyectos.Application**: comandos, consultas, manejadores (handlers), DTOs, validadores FluentValidation, mapeadores, interfaz de servicio y un Mediador personalizado (hand-rolled).
- **MetasYProyectos.Infrastructure**: persistencia local cifrada (JSON encriptado con DataProtection), servicios de Oracle (conexion, cadena de conexion, validacion en pasos) y repositorio.
- **MetasYProyectos.Web**: aplicacion MVC con controladores, vistas Razor, view models, autenticacion por cookies y recursos estaticos (Bootstrap 5.3.3, jQuery, fuentes Sora/Inter).
- **MetasYProyectos.Test**: proyecto de pruebas reservado, actualmente sin implementar (sin framework de pruebas referenciado).

### Solucion

La solucion utiliza el formato `.slnx` (nuevo formato XML de .NET) e incluye los 5 proyectos.

## Estado del proyecto

Proyecto en construccion activa. Las funcionalidades implementadas hasta el momento:

### Configuracion de conexion Oracle
- CRUD completo (Crear, Editar, Eliminar, Listar) desde interfaz web.
- Formulario con campos: Nombre, Servidor, Puerto, Servicio, Version Oracle, Tipo Conexion y Credenciales.
- Soporte para tres modos de conexion: **ServiceName**, **SID** y **TNS**.
- Soporte para multiples versiones de Oracle: 9, 10, 11, 19, 21.
- Persistencia local cifrada en archivo `Config/bd.config` usando **DataProtection**.
- Proteccion de password en UI: se muestra mascara de 8 puntos y se preserva el valor real al guardar.
- Validacion con **FluentValidation** a nivel de DTO.
- Validacion de dominio en la entidad `ConfiguracionBD` usando factory estatico `Crear()`.

### Prueba de conexion Oracle
- Prueba de conectividad directa desde la interfaz de edicion de configuracion.
- Construccion manual de la cadena de conexion segun el modo (ServiceName/SID/TNS).
- Retorna version de Oracle al conectarse exitosamente.

### Autenticacion administrativa local
- Sistema de autenticacion administrativa con password hasheado (**PBKDF2**, 100k iteraciones, SHA256).
- Flujo de configuracion inicial: crea password administrativo y genera codigo de recuperacion (12 caracteres hex).
- Login administrativo protege la seccion de Configuracion via cookies con expiracion de 2 horas.
- Recuperacion de password usando codigo de recuperacion.
- Credenciales almacenadas en `App_Data/admin.json` (excluido del repositorio via `.gitignore`).
- Comparacion con tiempo constante para prevenir ataques de timing.

### Autenticacion de usuario Oracle (parcial)
- Formulario de login de usuario con campos: Usuario, Contrasena, Base de datos (dropdown) y Vigencia.
- Pipeline de validacion en 4 pasos contra Oracle:
  1. Verificar que el usuario existe (`ALL_USERS`).
  2. Verificar licencia de la aplicacion (`CTRL_APP`).
  3. Verificar que el usuario esta activo (`USUARIOS_PCT`).
  4. Verificar permiso de ingreso (`USUARIOS_PRIVS`).
- Establecimiento automatico de `CURRENT_SCHEMA` y formato de fecha.
- **Nota**: el POST del login no invoca aun el comando de autenticacion via Mediador; esta pendiente de integrar.

### Estructura de Mediador (CQRS)
- Mediador personalizado con resolucion por reflexion (no usa el paquete NuGet MediatR).
- Comandos: `GuardarConfiguracion`, `EliminarConfiguracion`.
- Consultas: `ObtenerConfiguracion`, `ObtenerConfiguracionPorNombre`, `ProbarConexion`.
- Registro automatico de handlers y validadores via assembly scanning en `DependencyInjection.cs`.

### Interfaz web
- Layout principal con topbar, tipografia Sora/Inter, tema Bootstrap 5.3.3.
- Pantalla de login standalone con diseno split-screen (panel de marca + formulario).
- Panel de configuracion con tabla de conexiones, LED de estado y formulario en dos columnas.
- Paginas de administracion: Setup, Login, Reset y paginas de confirmacion.
- Pagina placeholder "En construccion" para la seccion MetasProyectos.
- Mensajes de feedback via `TempData` (exito/error).

## Requisitos

- .NET 10 SDK para desarrollo.
- .NET 10 Hosting Bundle en el servidor IIS (si se despliega en IIS).
- Acceso de red desde el servidor hacia la base de datos Oracle.
- Permisos de escritura para la aplicacion en las carpetas donde guarda configuracion, credenciales y llaves de proteccion.

## Ejecucion local

Desde la raiz del repositorio:

```powershell
dotnet restore
dotnet build
dotnet run --project MetasYProyectos.Web
```

Para ejecutar en modo desarrollo con observacion de cambios:

```powershell
dotnet watch run --project MetasYProyectos.Web
```

Para ejecutar pruebas:

```powershell
dotnet test
```

> Nota: el proyecto de pruebas existe pero aun no tiene implementaciones ni un framework de pruebas referenciado.

## Flujo de uso

1. Al iniciar, se redirige a la pantalla de login.
2. Acceder a `/admin` para configurar el password administrativo inicial.
3. Iniciar sesion como administrador para acceder a la seccion de Configuracion.
4. Crear o editar una configuracion de conexion a Oracle.
5. Usar "Probar conexion" para verificar la conectividad.
6. Cerrar sesion administrativa.

## Estructura de archivos importante

```
MetasYProyectos.Web/
├── Config/bd.config              # Configuracion cifrada de BD (generado localmente)
├── App_Data/admin.json           # Credenciales admin hasheadas (generado localmente)
├── DataProtection-Keys/          # Llaves de DataProtection (generado localmente)
├── wwwroot/
│   ├── css/site.css              # Estilos personalizados
│   ├── js/site.js                # JavaScript placeholder
│   └── lib/                      # Bootstrap 5.3.3, jQuery, plugins
├── Views/                        # 15 vistas Razor
├── Controllers/                  # 5 controladores
├── Models/                       # ViewModels y modelos
├── Services/AdminAuthService.cs  # Servicio de autenticacion admin
└── Program.cs                    # Punto de entrada y configuracion
```

## Despliegue en IIS

### 1. Publicar la aplicacion

```powershell
dotnet publish MetasYProyectos.Web -c Release -o .\publish
```

### 2. Preparar el servidor

1. Instalar IIS con el modulo ASP.NET Core.
2. Instalar el .NET 10 Hosting Bundle.
3. Reiniciar IIS: `iisreset`.

### 3. Configurar el sitio

1. Copiar el contenido de `publish/` a la ruta del servidor (ej. `C:\inetpub\wwwroot\MetasYProyectos`).
2. Crear el sitio o aplicacion en IIS Manager apuntando a esa ruta.
3. Asignar un Application Pool con **.NET CLR Version** en `No Managed Code` y modo integrado.
4. Dar permisos de lectura/escritura a la identidad del Application Pool sobre la carpeta (necesario para archivos cifrados, credenciales y llaves de DataProtection).
5. Verificar que exista `web.config` en la carpeta publicada.

### 4. Validar

1. Abrir la URL configurada en IIS.
2. Completar la configuracion inicial de administrador.
3. Configurar una conexion Oracle y probar la conectividad.

Si el sitio no inicia: revisar el Visor de eventos, confirmar el Hosting Bundle, permisos de carpeta y conectividad a Oracle.

## Archivos sensibles (no versionar)

Estos archivos se generan localmente y **no deben compartirse**:

- `Config/bd.config` — configuracion cifrada de bases de datos.
- `App_Data/admin.json` — credenciales administrativas hasheadas.
- `DataProtection-Keys/` — llaves de cifrado de DataProtection.

En despliegues productivos, mantenerlos protegidos con permisos limitados a la identidad de la aplicacion.
