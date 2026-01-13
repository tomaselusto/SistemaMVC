using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Configuracion
{
    public class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria>
    {
        //configuración de FluentAPI
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired();
            builder.Property(x => x.Descripcion).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
        }

        
    }
}
