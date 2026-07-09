# MetasYProyectos

Aplicacion web ASP.NET Core MVC desarrollada en .NET 10 para gestionar la configuracion de conexion a una base de datos Oracle y la administracion inicial del acceso al sistema.

El proyecto esta organizado con una arquitectura por capas:

- `MetasYProyectos.Domain`: entidades, validaciones y contratos del dominio.
- `MetasYProyectos.Application`: casos de uso, DTOs, validadores y logica de aplicacion.
- `MetasYProyectos.Infrastructure`: persistencia local cifrada, servicios de infraestructura y conexion Oracle.
- `MetasYProyectos.Web`: aplicacion MVC, controladores, vistas y recursos estaticos.
- `MetasYProyectos.Test`: proyecto reservado para pruebas.

## Estado del proyecto

Este proyecto se encuentra en construccion.

Actualmente incluye una base funcional para:

- Configuracion de conexion Oracle desde interfaz web.
- Prueba de conexion contra Oracle.
- Persistencia local cifrada de la configuracion.
- Autenticacion administrativa local.
- Estructura inicial de pruebas, pendiente de ampliar.

Al estar en desarrollo, la estructura, pantallas, flujos y reglas de negocio pueden cambiar.

## Requisitos

- .NET 10 SDK para desarrollo.
- .NET 10 Hosting Bundle en el servidor IIS.
- IIS habilitado con el modulo ASP.NET Core.
- Acceso de red desde el servidor hacia la base de datos Oracle.
- Permisos de escritura para la aplicacion en las carpetas locales donde guarda configuracion, credenciales y llaves de proteccion de datos.

## Ejecucion local

Desde la raiz del repositorio:

```powershell
dotnet restore
dotnet build
dotnet run --project MetasYProyectos.Web
```

Para ejecutar pruebas:

```powershell
dotnet test
```

> Nota: el proyecto de pruebas existe, pero aun esta pendiente completar cobertura funcional.

## Despliegue en IIS

### 1. Preparar el servidor

1. Instalar IIS en Windows Server o Windows.
2. Instalar el .NET 10 Hosting Bundle correspondiente al runtime usado por la aplicacion.
3. Reiniciar IIS despues de instalar el Hosting Bundle:

```powershell
iisreset
```

### 2. Publicar la aplicacion

Desde la raiz del repositorio, ejecutar:

```powershell
dotnet publish MetasYProyectos.Web -c Release -o .\publish
```

Esto genera los archivos listos para IIS en la carpeta:

```text
publish/
```

### 3. Copiar archivos al servidor

Copiar el contenido de la carpeta `publish` a una ruta del servidor, por ejemplo:

```text
C:\inetpub\wwwroot\MetasYProyectos
```

No copiar la carpeta `publish` como carpeta contenedora si se desea que esa ruta sea la raiz del sitio; copiar su contenido.

### 4. Crear el sitio en IIS

1. Abrir **Internet Information Services (IIS) Manager**.
2. Crear un nuevo sitio web o una aplicacion dentro de un sitio existente.
3. Seleccionar como ruta fisica la carpeta donde se copiaron los archivos publicados.
4. Configurar el puerto, host name o binding requerido.
5. Asignar un Application Pool propio para la aplicacion.

### 5. Configurar el Application Pool

En el Application Pool de la aplicacion:

1. Establecer **.NET CLR Version** en `No Managed Code`.
2. Usar modo integrado.
3. Definir la identidad que ejecutara la aplicacion.

### 6. Permisos de carpeta

Dar permisos de lectura y escritura a la identidad del Application Pool sobre la carpeta de la aplicacion publicada.

Esto es necesario porque la aplicacion puede crear o actualizar archivos locales para:

- Configuracion cifrada.
- Credenciales administrativas.
- Llaves de Data Protection.

Si el Application Pool se llama `MetasYProyectos`, la identidad normalmente sera:

```text
IIS AppPool\MetasYProyectos
```

### 7. Validar el archivo web.config

La publicacion debe generar un archivo `web.config`. Verificar que exista en la raiz publicada, ya que IIS lo utiliza para iniciar la aplicacion ASP.NET Core.

### 8. Probar el sitio

1. Abrir la URL configurada en IIS.
2. Completar el flujo inicial de administracion si aplica.
3. Configurar los datos de conexion Oracle.
4. Ejecutar la prueba de conexion desde la interfaz.

### 9. Revisar errores

Si el sitio no inicia:

1. Revisar el Visor de eventos de Windows.
2. Confirmar que el Hosting Bundle instalado corresponde a .NET 10.
3. Confirmar permisos de escritura en la carpeta publicada.
4. Validar que el servidor tenga conectividad hacia Oracle.
5. Revisar que `web.config` exista y que los archivos publicados esten completos.

## Archivos sensibles

No se deben versionar ni compartir archivos generados localmente con configuracion, credenciales o llaves de cifrado.

En despliegues productivos, estos archivos deben mantenerse protegidos y con permisos limitados a la identidad que ejecuta la aplicacion.
