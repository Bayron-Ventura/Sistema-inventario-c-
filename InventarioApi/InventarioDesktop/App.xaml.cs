using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using InventarioDesktop.Data;
using InventarioDesktop.Views;
using InventarioDesktop.ViewModels;

namespace InventarioDesktop
{
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            try
            {
                ServiceCollection services = new ServiceCollection();
                ConfigureServices(services);
                _serviceProvider = services.BuildServiceProvider();

                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fatal: {ex.Message}\n\n{ex.InnerException?.Message}", "Error de Inicio", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

private void ConfigureServices(ServiceCollection services)
{
    services.AddDbContext<InventarioContext>();
    
    services.AddSingleton<IServiceProvider>(provider => provider);
    
    services.AddTransient<MainViewModel>();
    services.AddTransient<CategoriaViewModel>();
    services.AddTransient<ProductoViewModel>();  
    
    services.AddTransient<MainWindow>();
    services.AddTransient<CategoriaWindow>();
    services.AddTransient<ProductoWindow>(); 
}

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Ocurrió una excepción no controlada: {e.Exception.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}