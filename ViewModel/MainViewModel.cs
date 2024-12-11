using gestion_productos.Data;
using gestion_productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace gestion_productos.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RepositorioProductos _repositorioProductos;
        private ObservableCollection<Productos> _products;
        public ObservableCollection<Productos> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public MainViewModel()
        {
            _repositorioProductos = new RepositorioProductos(@"Data Source=KAISER\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");
            Products = new ObservableCollection<Productos>();
        }

        public void AddProduct(Productos product)
        {
            _repositorioProductos.AddProduct(product);
            Products.Add(product);
            OnPropertyChanged(nameof(Products));
        }

        public void UpdateProduct(Productos product)
        {
            _repositorioProductos.UpdateProduct(product);
            var existingProduct = Products.FirstOrDefault(p => p.ID == product.ID);
            if (existingProduct != null)
            {
                existingProduct.Nombre = product.Nombre;
                existingProduct.Descripcion = product.Descripcion;
                existingProduct.Precio = product.Precio;
                existingProduct.Cantidad = product.Cantidad;
                OnPropertyChanged(nameof(Products));
            }
        }

        public void DeleteProduct(Productos product)
        {
            _repositorioProductos.DeleteProduct(product.ID);
            Products.Remove(product);
            OnPropertyChanged(nameof(Products));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
