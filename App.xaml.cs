using gestion_productos.Data;
using System;
using System.Windows;

namespace gestion_productos
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var context = new InventarioDBContext())
            {
                DataSeeder.Seed(context);
            }

        }
    }
}
