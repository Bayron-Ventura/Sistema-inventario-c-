using System;
using System.Windows;
using InventarioDesktop.Models;
using System.Collections.Generic;

namespace InventarioDesktop.Views
{
    public partial class ProductoDialogWindow : Window
    {
        public Producto? ProductoResult { get; private set; }
        private readonly Producto? _productoOriginal;

        public ProductoDialogWindow(Producto? producto = null, List<Categoria>? categorias = null)
        {
            InitializeComponent();
            
            _productoOriginal = producto;
            
            cmbCategoria.ItemsSource = categorias;
            
            // Modo Edición
            if (producto != null)
            {
                txtNombre.Text = producto.Nombre;
                txtDescripcion.Text = producto.Descripcion;
                txtPrecio.Text = producto.Precio.ToString("F2");
                txtStock.Text = producto.Stock.ToString();
                txtFechaCreacion.Text = producto.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss");
                
                cmbCategoria.SelectedValue = producto.CategoriaId;
                
                this.Title = "Editar Producto";
                txtTitulo.Text = "✏️ Editar Producto";
            }
            else
            {
                this.Title = "Agregar Nuevo Producto";
                txtTitulo.Text = "➕ Nuevo Producto";
                txtFechaCreacion.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Por favor, introduce un precio válido.", "Error de Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrecio.Focus();
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Por favor, introduce un stock válido.", "Error de Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStock.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del producto no puede estar vacío.", "Error de Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNombre.Focus();
                return;
            }

            if (cmbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona una categoría.", "Error de Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbCategoria.Focus();
                return;
            }

            ProductoResult = new Producto
            {
                Id = _productoOriginal?.Id ?? 0,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                Categoria = (Categoria)cmbCategoria.SelectedItem,
                CategoriaId = ((Categoria)cmbCategoria.SelectedItem).Id,
                Precio = precio,
                Stock = stock,
                FechaCreacion = DateTime.Parse(txtFechaCreacion.Text)
            };

            this.DialogResult = true;
            this.Close();
        }
    }
}