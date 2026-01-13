using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelos.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage= "El nombre es requerido")]
        [MaxLength(30, ErrorMessage ="El nombre no puede exceder los 30 caracteres")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "La descripción es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string? Descripcion { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
