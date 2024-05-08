using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AddDocument.xaml
    /// </summary>
    public partial class AddDocument : Window
    {
        public bool Result { get; private set; }
        public string CodiceDocumento { get; private set; }
        public string TipoDocumento { get; private set; }

        public AddDocument()
        {
            InitializeComponent();
        }

        private void btnConfermaDocumento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codiceDocumento = txtCodiceDocumento.Text.Trim();
                ComboBoxItem selectedItem = cmbTipoDocumento.SelectedItem as ComboBoxItem;
                string tipoDocumento = selectedItem.Content.ToString();
                if (string.IsNullOrEmpty(codiceDocumento))
                {
                    _ = txtCodiceDocumento.Focus();
                    throw new ArgumentNullException("Codice documento non inserito.");
                }
                Result = true;
                CodiceDocumento = codiceDocumento;
                TipoDocumento = tipoDocumento;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
