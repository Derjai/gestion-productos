using gestion_productos.Data;
using gestion_productos.Models;
using gestion_productos.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gestion_productos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RepositorioProductos _repositorioProductos;
        public MainWindow()
        {
            InitializeComponent();
            _repositorioProductos = new RepositorioProductos(@"Data Source=KAISER\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");
            LoadProducts();
        }

        private void LoadProducts()
        {
            var viewModel = (MainViewModel)this.DataContext;
            viewModel.Products = new ObservableCollection<Productos>(_repositorioProductos.GetProducts());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(ProductNameTextBox.Text) ||
                String.IsNullOrWhiteSpace(ProductPriceTextBox.Text) ||
                String.IsNullOrWhiteSpace(ProductQuantityTextBox.Text))
            {
                MessageBox.Show("Los campos 'Nombre', 'Precio' y 'Cantidad' son obligatorios");
                return;
            }

            if(!decimal.TryParse(ProductPriceTextBox.Text, out decimal price) || price <=0)
            {
                MessageBox.Show("El precio debe ser un número positivo");
                return;
            }

            if(!int.TryParse(ProductQuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número positivo");
                return;
            }

            Productos newProduct = new Productos
            {
                Nombre = ProductNameTextBox.Text,
                Descripcion = ProductDescriptionTextBox.Text ?? "Producto nuevo",
                Precio = price,
                Cantidad = quantity
            };

            _repositorioProductos.AddProduct(newProduct);
            ((MainViewModel)this.DataContext).Products.Add(newProduct);
            LoadProducts();
            ProductNameTextBox.Clear();
            ProductDescriptionTextBox.Clear();
            ProductPriceTextBox.Clear();
            ProductQuantityTextBox.Clear();
            MessageBox.Show("Producto añadido correctamente");
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(ProductsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un producto para editarlo");
                return;
            }

            Productos selectedProduct = (Productos)ProductsDataGrid.SelectedItem;
            ProductNameTextBox.Text = selectedProduct.Nombre;
            ProductDescriptionTextBox.Text = selectedProduct.Descripcion;
            ProductPriceTextBox.Text = selectedProduct.Precio.ToString();
            ProductQuantityTextBox.Text = selectedProduct.Cantidad.ToString();

            SaveButton.Content = "Actualizar";
            SaveButton.Tag = selectedProduct;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(ProductsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un producto para eliminarlo");
                return;
            }

            Productos selectedProduct = (Productos)ProductsDataGrid.SelectedItem;
            if (MessageBox.Show($"¿Estás seguro de eliminar el producto '{selectedProduct.Nombre}'?", "Eliminar producto", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _repositorioProductos.DeleteProduct(selectedProduct.ID);
                ((MainViewModel)this.DataContext).Products.Remove(selectedProduct);
                LoadProducts();
                MessageBox.Show("Producto eliminado correctamente");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
           if(string.IsNullOrWhiteSpace(ProductNameTextBox.Text)||
              string.IsNullOrWhiteSpace(ProductPriceTextBox.Text) ||
              string.IsNullOrWhiteSpace(ProductQuantityTextBox.Text) ||
              string.IsNullOrWhiteSpace(ProductDescriptionTextBox.Text))
            {
                MessageBox.Show("Complete los campos");
                return;
            }

            if (!decimal.TryParse(ProductPriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("El precio debe ser un número positivo");
                return;
            }

            if (!int.TryParse(ProductQuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número positivo");
                return;
            }

            if(SaveButton.Tag is Productos productToUpdate)
            {
                productToUpdate.Nombre = ProductNameTextBox.Text;
                productToUpdate.Descripcion = ProductDescriptionTextBox.Text;
                productToUpdate.Precio = price;
                productToUpdate.Cantidad = quantity;

                _repositorioProductos.UpdateProduct(productToUpdate);
                ((MainViewModel)this.DataContext).UpdateProduct(productToUpdate);
                LoadProducts();
                SaveButton.Content = "Guardar ";
                SaveButton.Tag = null;

                MessageBox.Show("Producto actualizado correctamente");
                ProductNameTextBox.Clear();
                ProductDescriptionTextBox.Clear();
                ProductPriceTextBox.Clear();
                ProductQuantityTextBox.Clear();
            }
        }
    }
}
