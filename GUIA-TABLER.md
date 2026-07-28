# Guia de Tabler — Metas y Proyectos

## Arquitectura frontend

- **Tabler v1.4.0** — archivos locales en `wwwroot/lib/tabler/dist/`
- **Bootstrap 5** incluido (Tabler lo trae embebido)
- **Sin build pipeline** — CSS puro, sin SCSS/SASS/Webpack
- Layout principal: `Views/Shared/_Layout.cshtml`

## Variables de color base

Definidas en `wwwroot/css/site.css`:

```css
:root {
    /* Colores propios (usados en CSS custom) */
    --color-primary: #011C6B;
    --color-primary-light: #0A2B8C;
    --color-primary-soft: #E8EBF7;
    --color-bg: #F4F6FB;
    --color-success: #1FA37E;
    --color-danger: #D64550;

    /* Overrides de Tabler */
    --tblr-primary: #011C6B;    /* color principal de todo Tabler */
    --tblr-primary-l: 40;       /* luminosidad (0-100) */
}
```

### Cambiar el color primario global

Cambiar ambos `--tblr-primary` y `--color-primary` al mismo valor:

```css
:root {
    --tblr-primary: #NUEVO_COLOR;
    --color-primary: #NUEVO_COLOR;
}
```

Esto afecta: botones `btn-primary`, links activos del sidebar, focus states de Tabler, y cualquier elemento que use `--color-primary`.

### Cambiar colores semánticos

| Variable | Controla |
|---|---|
| `--color-primary-light` | Hover de botones primary |
| `--color-primary-soft` | Fondo suave (definida, no consumida aun) |
| `--color-bg` | Color de fondo general (definida, no consumida aun) |
| `--color-success` | Punto LED verde, indicadores |
| `--color-danger` | Toasts de error |

## Componentes disponibles

### Tarjetas (Cards)

```html
<div class="card">
    <div class="card-header">
        <h5 class="card-title">Titulo</h5>
    </div>
    <div class="card-body">
        Contenido
    </div>
</div>
```

Variante con estilo custom (formulario):
```html
<div class="config-card card">
    <div class="card-header">...</div>
    <div class="card-body">...</div>
</div>
```

### Tablas

```html
<div class="table-responsive">
    <table class="table table-hover table-vcenter card-table">
        <thead>
            <tr><th>Col 1</th><th>Col 2</th></tr>
        </thead>
        <tbody>
            <tr><td>Dato</td><td>Dato</td></tr>
        </tbody>
    </table>
</div>
```

### Formularios

```html
<div class="row g-3">
    <div class="col-md-6">
        <label class="form-label">Campo</label>
        <input type="text" class="form-control" placeholder="..." />
        <span class="text-danger small">Error aqui</span>
    </div>
    <div class="col-md-6">
        <label class="form-label">Select</label>
        <select class="form-select">
            <option>Opcion 1</option>
        </select>
    </div>
</div>
```

Input con icono (input-group):
```html
<div class="input-group">
    <span class="input-group-text bg-white"><i class="ti ti-user"></i></span>
    <input class="form-control" />
</div>
```

### Botones

```html
<button class="btn btn-primary">Principal</button>
<button class="btn btn-outline-primary">Outline</button>
<button class="btn btn-sm btn-outline-danger">Peligro pequeno</button>
<button class="btn btn-warning">Advertencia</button>
<a class="btn btn-link">Link</a>
```

### Alertas

```html
<div class="alert alert-success">
    <i class="ti ti-check-circle me-1"></i> Mensaje exito
</div>
<div class="alert alert-danger">
    <i class="ti ti-alert-triangle me-1"></i> Mensaje error
</div>
```

### Iconos (Tabler Icons)

Clase base: `ti ti-{nombre}`. Algunos usados en el proyecto:

| Icono | Clase |
|---|---|
| Inicio | `ti ti-home` |
| Base de datos | `ti ti-database` |
| Agregar | `ti ti-plus` |
| Editar | `ti ti-pencil` |
| Eliminar | `ti ti-trash` |
| Guardar | `ti ti-device-floppy` |
| Flecha izq | `ti ti-arrow-left` |
| Flecha der | `ti ti-arrow-right` |
| Configuracion | `ti ti-settings` |
| Cerrar sesion | `ti ti-logout` |
| Conectar | `ti ti-plug` |
| Ver password | `ti ti-eye` |
| Ocultar password | `ti ti-eye-closed` |
| Bloqueado | `ti ti-lock` |
| Check | `ti ti-check-circle` |
| Alerta | `ti ti-alert-triangle` |

Lista completa: https://tabler.io/icons

### Navegacion (Sidebar)

El sidebar usa `data-bs-theme="dark"` para tema oscuro automatico:

```html
<aside class="navbar navbar-vertical navbar-expand-sm" data-bs-theme="dark">
    <ul class="navbar-nav pt-lg-3">
        <li class="nav-item active">
            <a class="nav-link" href="/">
                <span class="nav-link-icon"><i class="ti ti-home"></i></span>
                <span class="nav-link-title">Inicio</span>
            </a>
        </li>
    </ul>
</aside>
```

## Layout de pagina

```html
<div class="page">
    <aside class="navbar navbar-vertical navbar-expand-sm" data-bs-theme="dark">
        <!-- sidebar -->
    </aside>
    <div class="page-wrapper">
        <div class="page-header d-print-none">
            <div class="container-xl">
                <div class="page-pretitle">Subtitulo</div>
                <h2 class="page-title">Titulo principal</h2>
            </div>
        </div>
        <div class="page-body">
            <div class="container-xl">
                @RenderBody()
            </div>
        </div>
    </div>
</div>
```

## Agregar un componente nuevo

1. Agregar HTML en la vista `.cshtml` usando las clases de Tabler/Bootstrap
2. Iconos disponibles con `ti ti-{nombre}` (ver tabler.io/icons)
3. Para estilos custom: agregar reglas en `wwwroot/css/site.css` usando las variables CSS existentes
4. No hay pipeline de build — los cambios CSS se reflejan al recargar

## Agregar una pagina standalone (sin layout)

```html
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <link rel="stylesheet" href="~/lib/tabler/dist/css/tabler.min.css" />
    <link rel="stylesheet" href="~/lib/tabler/dist/css/tabler-icons.min.css" />
</head>
<body>
    <!-- contenido -->
</body>
</html>
```

Nota: las paginas standalone no cargan `tabler.min.js` (no necesitan componentes JS).

## Sobreescribir estilos de Tabler

Agregar en `site.css` despues de los overrides actuales:

```css
/* Sobreescribir componente especifico de Tabler */
:root {
    --tblr-{variable}: #valor;
}
```

Variables Tabler disponibles: https://tabler.io/docs/getting-started#theming
