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
