using System.Windows;

namespace InventarioDesktop.Views
{
    public partial class InputDialog : Window
    {
        public string ResponseText { get; private set; } = string.Empty;

        public InputDialog(string prompt)
        {
            InitializeComponent();
            lblPrompt.Content = prompt;
            txtInput.Focus();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = txtInput.Text;
            this.DialogResult = true;
            this.Close();
        }

        public static string? Show(Window owner, string prompt)
        {
            var dialog = new InputDialog(prompt);
            dialog.Owner = owner;
            return dialog.ShowDialog() == true ? dialog.ResponseText : null;
        }
    }
}