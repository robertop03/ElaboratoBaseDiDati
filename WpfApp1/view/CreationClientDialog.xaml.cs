using System;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per CreationClientDialog.xaml
    /// </summary>
    public partial class CreationClientDialog : Window
    {
        public bool Result { get; private set; }
        public string CodiceFiscale { get; private set; }
        public string Nome { get; private set; }
        public string Cognome { get; private set; }
        public string NumeroTelefono { get; private set; }
        public string Città { get; private set; }
        public string Via { get; private set; }
        public int NumeroCivico { get; private set; }
        public string Email { get; private set; }

        public CreationClientDialog(DateTime dataInizio, DateTime dataFine)
        {
            InitializeComponent();
            _ = txtCodiceFiscale.Focus();
            TimeSpan durata = dataFine - dataInizio;
            if (durata.Days >= 30)
            {
                grbDocumento.Visibility = Visibility.Visible;
            }
            else
            {
                Height -= grbDocumento.Height;
                grbDocumento.Visibility = Visibility.Collapsed;
            }
        }

        private void btnConfermaCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show();
            }

            Result = true;
            Close();
        }

        private void btnAnnullaCliente_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}
