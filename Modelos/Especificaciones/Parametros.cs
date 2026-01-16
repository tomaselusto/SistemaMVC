using System;
using System.Collections.Generic;
using System.Text;

namespace Modelos.Especificaciones
{
    public class Parametros
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;  //cantidad de registros por pág.
    }
}
