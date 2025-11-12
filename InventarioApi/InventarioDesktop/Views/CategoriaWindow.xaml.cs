using System.Windows;
using InventarioDesktop.ViewModels;

namespace InventarioDesktop.Views
{
    public partial class CategoriaWindow : Window
    {
        private CategoriaViewModel? _viewModel;

        public CategoriaWindow(CategoriaViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Nos suscribimos al evento Loaded de la ventana
            this.Loaded += CategoriaWindow_Loaded;
        }

        // Este evento se dispara cuando la ventana ya está visible y lista para interactuar
        private async void CategoriaWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Ahora sí, llamamos al método asíncrono para cargar los datos
            // Esto no bloqueará la interfaz de usuario
            await _viewModel!.LoadCategorias();
        }
    }
}