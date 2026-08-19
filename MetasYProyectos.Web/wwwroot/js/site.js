// ponytail: Toastify helper, single global function
var MostrarNotificacion = function (mensaje, tipo) {
    var colores = {
        success: '#1FA37E',
        error: '#D64550',
        warning: '#F5A623',
        info: '#0d6efd'
    };
    Toastify({
        text: mensaje,
        duration: 5000,
        gravity: 'top',
        position: 'right',
        backgroundColor: colores[tipo] || colores.info,
        stopOnFocus: true
    }).showToast();
};

(function () {
    var keys = ['Exito', 'Error', 'Advertencia', 'Info'];
    var tipos = ['success', 'error', 'warning', 'info'];
    for (var i = 0; i < keys.length; i++) {
        var val = document.querySelector('input[data-toast="' + keys[i] + '"]');
        if (val && val.value) MostrarNotificacion(val.value, tipos[i]);
    }
})();

// Spinner de carga para navegacion interna y envio de formularios.
(function () {
    var overlayId = 'spinner-overlay';

    function overlay() {
        return document.getElementById(overlayId);
    }

    function setText(texto) {
        var textoEl = document.querySelector('#' + overlayId + ' .spinner-texto');
        if (textoEl && texto) textoEl.textContent = texto;
    }

    function mostrar(texto) {
        var s = overlay();
        if (!s) return;
        setText(texto);
        s.style.display = 'flex';
    }

    function ocultar() {
        var s = overlay();
        if (!s) return;
        s.style.display = 'none';
    }

    function esNavegacionInterna(link) {
        if (!link || link.dataset.spinner === 'false') return false;
        if (link.target && link.target !== '_self') return false;
        if (link.hasAttribute('download')) return false;
        if (link.href.startsWith('javascript:') || link.href.startsWith('mailto:') || link.href.startsWith('tel:')) return false;

        var url = new URL(link.href, window.location.href);
        return url.origin === window.location.origin && url.href !== window.location.href && !url.hash;
    }

    window.MetasSpinner = {
        mostrar: mostrar,
        ocultar: ocultar
    };

    ocultar();
    window.addEventListener('pageshow', ocultar);

    document.addEventListener('submit', function (e) {
        var form = e.target;
        if (e.defaultPrevented) return;
        if (!form || form.dataset.spinner === 'false') return;
        if (typeof form.checkValidity === 'function' && !form.checkValidity()) return;
        mostrar(form.dataset.spinnerTexto || 'Procesando...');
    });

    document.addEventListener('click', function (e) {
        var link = e.target.closest && e.target.closest('a[href]');
        if (!esNavegacionInterna(link)) return;
        mostrar(link.dataset.spinnerTexto || 'Cargando...');
    });
})();

// ponytail: sidebar collapse toggle
(function () {
    var sidebar = document.querySelector('.navbar-vertical');
    var toggle = document.querySelector('.sidebar-toggle');
    var icon = toggle && toggle.querySelector('.sidebar-toggle-icon');
    if (!sidebar || !toggle) return;
    // restore saved state
    if (localStorage.getItem('sidebar-collapsed') === 'true') {
        sidebar.classList.add('collapsed');
        if (icon) icon.className = 'ti ti-layout-sidebar-right-collapse sidebar-toggle-icon';
    }
    toggle.addEventListener('click', function () {
        var collapsed = sidebar.classList.toggle('collapsed');
        if (icon) icon.className = collapsed
            ? 'ti ti-layout-sidebar-right-collapse sidebar-toggle-icon'
            : 'ti ti-layout-sidebar-left-collapse sidebar-toggle-icon';
        localStorage.setItem('sidebar-collapsed', collapsed);
    });
})();
