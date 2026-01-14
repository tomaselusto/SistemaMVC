using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Configuracion
{
    public class MarcaConfiguracion
    {
        //configuración de FluentAPI
        public void Configure(EntityTypeBuilder<Marca> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired();
            builder.Property(x => x.Descripcion).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
        }
    }
}
