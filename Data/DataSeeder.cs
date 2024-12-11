using gestion_productos.Models;
using System.Linq;

namespace gestion_productos.Data
{
    public static class DataSeeder
    {
        public static void Seed(InventarioDBContext context)
        {
            context.Database.CreateIfNotExists();
            if (!context.Productos.Any())
            {
                context.Productos.AddRange(new[]
                {
                    new Productos { Nombre = "Laptop", Descripcion = "Computadora portátil", Precio = 1200.99m, Cantidad = 10 },
                    new Productos { Nombre = "Mouse", Descripcion = "Mouse inalámbrico", Precio = 25.50m, Cantidad = 50 },
                });
                context.SaveChanges();
            }
        }
    }
}
