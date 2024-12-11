using gestion_productos.Models;
using System.Collections.ObjectModel;

namespace gestion_productos.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<Productos> Products { get; set; }

        public MainViewModel()
        {
            Products = new ObservableCollection<Productos>();
            LoadProducts();
        }

        private void LoadProducts()
        {
            Products.Add(new Productos { ID = 1, Nombre = "Producto 1", Descripcion = "Descripción 1", Precio = 10.5m, Cantidad = 100 });
            Products.Add(new Productos { ID = 2, Nombre = "Producto 2", Descripcion = "Descripción 2", Precio = 20.0m, Cantidad = 50 });
        }
    }
}
