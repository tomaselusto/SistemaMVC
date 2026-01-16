using System;
using System.Collections.Generic;
using System.Text;

namespace Modelos.Especificaciones
{
    public class MetaData
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }  //total de páginas
        public int TotalCount { get; set; } //total de registros
    }
}
