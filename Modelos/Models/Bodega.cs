using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelos.Models
{
    public class Bodega
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es requerido")]
        [MaxLength(40, ErrorMessage="Nombre máximo 40 caracteres")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "la descripción es requerido")]
        [MaxLength(100, ErrorMessage = "Nombre máximo 100 caracteres")]
        public string? Descripcion { get; set; }
        public bool Estado { get; set; } = true;
    }
}
