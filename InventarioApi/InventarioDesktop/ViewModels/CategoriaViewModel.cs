using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using InventarioDesktop.Data;
using InventarioDesktop.Models;
using InventarioDesktop.Views;

namespace InventarioDesktop.ViewModels
{
    public class CategoriaViewModel : BaseViewModel
    {
        private readonly InventarioContext _context;
        private Categoria? _selectedCategoria;

        public ObservableCollection<Categoria> Categorias { get; }
        public Categoria? SelectedCategoria { get => _selectedCategoria; set { _selectedCategoria = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public CategoriaViewModel(InventarioContext context)
        {
            _context = context;
            Categorias = new ObservableCollection<Categoria>();
            
            AddCommand = new RelayCommand(_ => Add());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedCategoria != null);
            
            RefreshCommand = new RelayCommand(async _ => await LoadCategorias());
        }

        public async Task LoadCategorias()
        {
            Categorias.Clear();
            var categorias = await _context.Categorias.ToListAsync();
            foreach (var cat in categorias)
            {
                Categorias.Add(cat);
            }
        }

        private async void Add()
        {
            var nombre = InputDialog.Show(Application.Current.MainWindow, "Nombre de la nueva categoría:");
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var nuevaCategoria = new Categoria { Nombre = nombre };
                _context.Categorias.Add(nuevaCategoria);
                await _context.SaveChangesAsync();
                await LoadCategorias();
            }
        }

        private async void Delete()
        {
            if (SelectedCategoria != null)
            {
                var result = MessageBox.Show($"¿Estás seguro de que quieres eliminar la categoría '{SelectedCategoria.Nombre}'?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Categorias.Remove(SelectedCategoria);
                    await _context.SaveChangesAsync();
                    await LoadCategorias();
                }
            }
        }
    }
}