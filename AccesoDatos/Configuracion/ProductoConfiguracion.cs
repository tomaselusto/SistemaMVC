using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Configuracion
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    {
        //configuración de FluentAPI
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.NumeroSerie).IsRequired();
            builder.Property(x => x.Descripcion).IsRequired();
            builder.Property(x => x.Estado).IsRequired();

            builder.Property(x => x.Precio).IsRequired();
            builder.Property(x => x.Costo).IsRequired();
            builder.Property(x => x.CateogriaId).IsRequired();  //acá siempre se pone el nombre de la propiedad y no el de la navegacion (Marca Marca)
            builder.Property(x => x.MarcaId).IsRequired();

            //campos no requeridos

            builder.Property(x => x.ImagenUrl).IsRequired(false);
            builder.Property(x => x.PadreId).IsRequired(false);

            //relaciones
            //De uno a muchos                                                             para no tener problemas con el borrado en cascada      
            builder.HasOne(x => x.Categoria).WithMany().HasForeignKey(x => x.CateogriaId).OnDelete(DeleteBehavior.NoAction); //acá hacemos la relacion y la navegación

            builder.HasOne(x => x.Marca).WithMany().HasForeignKey(x => x.MarcaId).OnDelete(DeleteBehavior.NoAction);

            //recursividad
            builder.HasOne(x => x.Padre).WithMany().HasForeignKey(x => x.PadreId).OnDelete(DeleteBehavior.NoAction);


                 

        }
    }
}
