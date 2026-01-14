using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Modelos.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage ="Debe ingresar un número de serie")]
        [MaxLength (50)]
        public string NumeroSerie { get; set; }
        [Required(ErrorMessage = "Debe ingresar una descripción")]
        [MaxLength(50)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage ="El precio es necesario")]
        public double Precio { get; set; }
        [Required(ErrorMessage = "El costo es necesario")]
        public double Costo { get; set; }

        public string ImagenUrl { get; set; }
        [Required(ErrorMessage = "El estado es necesario")]
        public bool Estado { get; set; }

        //foreign Key
        [Required(ErrorMessage = "La categoria es necesario")]
        public int CateogriaId { get; set; } //relación con categoria
        [ForeignKey("CateogriaId")]  //acá hago la relación del campo, con el objeto
        public Categoria Categoria { get; set; }//navegacion (a qué modelo quiero relacionar categoriaId)
        [Required(ErrorMessage = "La marca es necesario")]
        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")] //debe ser el mismo nombre dela propiedad
        public Marca Marca { get; set; }


        //recursividad por si algun producto está relacionado con otro producto
        public int? PadreId { get; set; }  //int? puede ser nulo (sin el ? se guarda como 0)
        public virtual Producto Padre { get; set; } //un producto puede estar relacionado a un mismo producto

    }
}
