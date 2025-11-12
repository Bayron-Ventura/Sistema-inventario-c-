using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using InventarioDesktop.Views;

namespace InventarioDesktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private Window? _mainWindow;
        private WindowState _previousWindowState;
        private WindowStyle _previousWindowStyle;
        private bool _isFullScreen = false;

        public ICommand OpenCategoriasCommand { get; }
        public ICommand OpenProductosCommand { get; }
        public ICommand ToggleFullScreenCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            OpenCategoriasCommand = new RelayCommand(_ => OpenCategorias());
            OpenProductosCommand = new RelayCommand(_ => OpenProductos());
            ToggleFullScreenCommand = new RelayCommand(_ => ToggleFullScreen());
        }

        public void SetWindow(Window window)
        {
            _mainWindow = window;
            _previousWindowState = window.WindowState;
            _previousWindowStyle = window.WindowStyle;
        }

        private void OpenCategorias()
        {
            var window = _serviceProvider.GetRequiredService<CategoriaWindow>();
            window.ShowDialog();
        }

        private void OpenProductos()
        {
            var window = _serviceProvider.GetRequiredService<ProductoWindow>();
            window.ShowDialog();
        }

        private void ToggleFullScreen()
        {
            if (_mainWindow == null) return;

            if (_isFullScreen)
            {
                _mainWindow.WindowState = _previousWindowState;
                _mainWindow.WindowStyle = _previousWindowStyle;
                _mainWindow.ResizeMode = ResizeMode.CanResize;
                _isFullScreen = false;
            }
            else
            {
                _previousWindowState = _mainWindow.WindowState;
                _previousWindowStyle = _mainWindow.WindowStyle;
                
                _mainWindow.WindowStyle = WindowStyle.None;
                _mainWindow.WindowState = WindowState.Maximized;
                _mainWindow.ResizeMode = ResizeMode.NoResize;
                _isFullScreen = true;
            }
        }
    }
}