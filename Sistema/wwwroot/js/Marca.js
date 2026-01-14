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
            "url":"/Admin/Marca/ObtenerTodos"
        },
        /*renderziamos esta sección por columnas*/
        "columns": [
            { "data": "nombre", "width": "20%" },
            { "data": "descripcion", "width": "40%" },
            {
                "data": "estado",
                "render": function (data) {
                    if (data == true) {
                        return "Activo";
                    }
                    else {
                        return "Inactivo";
                    }
                }, "width": "20%"
            
            },
            {
                "data": "id", "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href= "/Admin/Marca/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>  

                            <a onclick=Delete("/Admin/Marca/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                            </a>

                            </div>

                    ` /*alt+96 para las comillas y renderizar codigo html*/
                },"width":"20%"
            }
        ]

    });
}


function Delete(url) {
    /*sweet alert y paramétros principales*/
    swal({
        title: "¿Está seguro de eliminar la marca?",
        text: "Este registro no se podrá ser recuperado",
        icon: "warning",
        buttons: true,
        dangerMode: true

    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
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
    });
}