using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using InventarioDesktop.Data;
using InventarioDesktop.Models;
using InventarioDesktop.Views;

namespace InventarioDesktop.ViewModels
{
    public class ProductoViewModel : BaseViewModel
    {
        private readonly InventarioContext _context;
        private Producto? _selectedProducto;
        private string _searchText = string.Empty;
        private ObservableCollection<Producto> _allProductos;

        public ObservableCollection<Producto> Productos { get; }
        
        public Producto? SelectedProducto 
        { 
            get => _selectedProducto; 
            set 
            { 
                _selectedProducto = value; 
                OnPropertyChanged(); 
            } 
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterProductos();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }

        public ProductoViewModel(InventarioContext context)
        {
            _context = context;
            Productos = new ObservableCollection<Producto>();
            _allProductos = new ObservableCollection<Producto>();
            
            AddCommand = new RelayCommand(_ => Add());
            EditCommand = new RelayCommand(_ => Edit(), _ => SelectedProducto != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedProducto != null);
            RefreshCommand = new RelayCommand(async _ => await LoadProductos());
            SearchCommand = new RelayCommand(_ => FilterProductos());
        }

        public async Task LoadProductos()
        {
            _allProductos.Clear();
            Productos.Clear();
            
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();
            
            foreach (var producto in productos)
            {
                _allProductos.Add(producto);
                Productos.Add(producto);
            }
        }

        private void FilterProductos()
        {
            Productos.Clear();
            
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allProductos
                : _allProductos.Where(p => p.Nombre.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            
            foreach (var producto in filtered)
            {
                Productos.Add(producto);
            }
        }

        private async void Add()
        {
            var categorias = await _context.Categorias.ToListAsync();
            
            if (categorias.Count == 0)
            {
                MessageBox.Show("No hay categorías disponibles. Por favor, crea al menos una categoría primero.", 
                    "Sin Categorías", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new ProductoDialogWindow(null, categorias);
            
            if (dialog.ShowDialog() == true && dialog.ProductoResult != null)
            {
                _context.Productos.Add(dialog.ProductoResult);
                await _context.SaveChangesAsync();
                await LoadProductos();
            }
        }

        private async void Edit()
        {
            if (SelectedProducto != null)
            {
                var categorias = await _context.Categorias.ToListAsync();
                
                var dialog = new ProductoDialogWindow(SelectedProducto, categorias);
                
                if (dialog.ShowDialog() == true && dialog.ProductoResult != null)
                {
                    SelectedProducto.Nombre = dialog.ProductoResult.Nombre;
                    SelectedProducto.Descripcion = dialog.ProductoResult.Descripcion;
                    SelectedProducto.Categoria = dialog.ProductoResult.Categoria;
                    SelectedProducto.CategoriaId = dialog.ProductoResult.Categoria?.Id ?? SelectedProducto.CategoriaId;
                    SelectedProducto.Precio = dialog.ProductoResult.Precio;
                    SelectedProducto.Stock = dialog.ProductoResult.Stock;
                    
                    await _context.SaveChangesAsync();
                    await LoadProductos();
                }
            }
        }

        private async void Delete()
        {
            if (SelectedProducto != null)
            {
                var result = MessageBox.Show(
                    $"¿Estás seguro de que quieres eliminar el producto '{SelectedProducto.Nombre}'?",
                    "Confirmar Eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.Yes)
                {
                    _context.Productos.Remove(SelectedProducto);
                    await _context.SaveChangesAsync();
                    await LoadProductos();
                }
            }
        }
    }
}