let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        /*seccion LENGUAJE*/
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        /*seccion AJAX*/
        "ajax": {
            "url": "/Admin/Usuario/ObtenerTodos"
        },
        /*renderziamos esta sección por columnas*/
        "columns": [
            { "data": "email",},
            { "data": "nombre" },
            { "data": "apellido" },
            { "data": "phoneNumber" },
            { "data": "rol" },
            {
                /*trabajar con 2 columnas al mismo tiempo en data*/
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    const hoy = new Date().getTime();
                    const bloqueo = data.lockoutEnd ? new Date(data.lockoutEnd).getTime() : 0; /*fecha de bloqueo*/
                    if (bloqueo > hoy) {
                        /*usuario loqueado*/
                        return `
              <div class="text-center">
                <button type="button" class="btn btn-danger text-white" style="width:150px"
                        onclick="BloquearDesbloquear('${data.id}')">
                  <i class="bi bi-unlock-fill"></i> Desbloquear
                </button>
              </div>`; /*alt+96 para las comillas y renderizar codigo html*/
                    }
                    else {
                        return `
              <div class="text-center">
                <button type="button" class="btn btn-success text-white" style="width:150px"
                        onclick="BloquearDesbloquear('${data.id}')">
                  <i class="bi bi-lock-fill"></i> Bloquear
                </button>
              </div>`;
                    
                    }
                    

                }, 
            }
        ]

    });
}


function BloquearDesbloquear(id) {
   
            $.ajax({
                type: "POST",
                url: '/Admin/Usuario/BloquearDesbloquear',
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                /*este success es el que mandamos en el json en el controlador*/
                success: function (data) {
                    if (data.success) {
                        /*libreria toastr para enviar notificaciones*/
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
       
}