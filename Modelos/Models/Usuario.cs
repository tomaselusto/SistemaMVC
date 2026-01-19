using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Modelos.Models
{
    public class Usuario : IdentityUser
    {
        //propiedas extras para usuario
        [Required(ErrorMessage ="El nombre es requerido")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        [MaxLength(50)]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "La dirección es requerida")]
        [MaxLength(150)]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "La ciudad es requerida")]
        [MaxLength(60)]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "El Pais es requerido")]
        [MaxLength(50)]
        public string Pais { get; set; }

        [NotMapped]  // esta propiedad no se agrega como columna en la bd
        public string Rol { get; set; } //relacionada con los permisos, es referencial.


    }
}
