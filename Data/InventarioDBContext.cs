using gestion_productos.Models;
using System.Data.Entity;

namespace gestion_productos.Data
{
    public class InventarioDBContext: DbContext
    {
        public InventarioDBContext() : base("InventarioDBContext")
        {
        }
        public DbSet<Productos> Productos { get; set; }
    }
}
