using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per ConfirmCreationUmbrella.xaml
    /// </summary>
    public partial class ConfirmCreationUmbrella : Window
    {
        public double PrezzoGiornaliero { get; private set; } = double.NaN;
        public bool Result { get; private set; }

        public ConfirmCreationUmbrella()
        {
            InitializeComponent();
            _ = txtPrezzo.Focus();
            txtPrezzo.SelectAll();
        }

        private void btnConferma_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtPrezzo.Text, out double prezzo) && prezzo >= 5 && prezzo <= 100)
            {
                PrezzoGiornaliero = prezzo;
                DialogResult = true;
            }
            else
            {
                _ = MessageBox.Show("Il prezzo deve essere un numero positivi >= 5 e <= 100.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Result = true;
            Close();
        }

        private void btnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}
