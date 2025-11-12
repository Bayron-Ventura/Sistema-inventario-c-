using System.Windows;
using InventarioDesktop.ViewModels;

namespace InventarioDesktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            viewModel.SetWindow(this);
        }
    }
}