using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modelos.ViewModels
{
    public class ProductoVM //VM-ViewModel
    {
        //viewmodel es un objeto que tiene otros objetos

        public Producto Producto { get; set; }

        public IEnumerable<SelectListItem> CategoriaLista { get; set; }

        public IEnumerable<SelectListItem> MarcaLista { get; set; }


    }
}
