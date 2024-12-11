using gestion_productos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace gestion_productos.Data
{
    public class RepositorioProductos
    {
        private string _connectionString;
        public RepositorioProductos(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Productos> GetProducts()
        {
            List<Productos> productos = new List<Productos>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Productos", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(new Productos
                    {
                        ID = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Precio = reader.GetDecimal(3),
                        Cantidad = reader.GetInt32(4)
                    });
                }
            }
            return productos;
        }

        public bool ProductExists(string productName, int? id = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Productos WHERE Nombre = @Nombre" + (id.HasValue ? " AND ID ! = @ID":""), connection);
                command.Parameters.AddWithValue("@Nombre", productName);
                if (id.HasValue)
                {
                    command.Parameters.AddWithValue("@ID", id.Value);
                }
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public void AddProduct(Productos product)
        {
            if (ProductExists(product.Nombre))
            {
                throw new Exception("Ya existe un producto con ese nombre");
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Productos (Nombre, Descripcion, Precio, Cantidad) VALUES (@Nombre, @Descripcion, @Precio, @Cantidad)", connection);
                command.Parameters.AddWithValue("@nombre", product.Nombre);
                command.Parameters.AddWithValue("@descripcion", product.Descripcion);
                command.Parameters.AddWithValue("@precio", product.Precio);
                command.Parameters.AddWithValue("@cantidad", product.Cantidad);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Productos product)
        {
            if(ProductExists(product.Nombre, product.ID))
            {
                throw new Exception("Ya existe un producto con ese nombre");
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Productos SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio, Cantidad = @Cantidad WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@nombre", product.Nombre);
                command.Parameters.AddWithValue("@descripcion", product.Descripcion);
                command.Parameters.AddWithValue("@precio", product.Precio);
                command.Parameters.AddWithValue("@cantidad", product.Cantidad);
                command.Parameters.AddWithValue("@id", product.ID);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int productId)
        {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Productos WHERE ID = @ID", connection);
                    command.Parameters.AddWithValue("@ID", productId);
                    int rowsAffected = command.ExecuteNonQuery();

                    if(rowsAffected == 0)
                    {
                        throw new Exception("No se encontró el producto");
                    }
                    else { Console.WriteLine("Producto eliminado"); }
                }
            }
        }
}
