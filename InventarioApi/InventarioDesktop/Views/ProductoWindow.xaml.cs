using System.Windows;
using InventarioDesktop.ViewModels;

namespace InventarioDesktop.Views
{
    public partial class ProductoWindow : Window
    {
        private ProductoViewModel? _viewModel;

        public ProductoWindow(ProductoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            
            this.Loaded += ProductoWindow_Loaded;
        }

        private async void ProductoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                await _viewModel.LoadProductos();
            }
        }
    }
}