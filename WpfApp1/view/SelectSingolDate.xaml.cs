using System;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per SelectSingolDate.xaml
    /// </summary>
    public partial class SelectSingolDate : Window
    {
        public DateTime Data { get; private set; }
        public string Pasto { get; private set; }
        public int NumeroPersonePrenotanti { get; private set; }

        public SelectSingolDate()
        {
            InitializeComponent();
            int year = DateTime.Now.Year;
            dtpData.DisplayDateStart = new DateTime(year, 6, 1);
            dtpData.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpData.SelectedDate = dtpData.DisplayDateStart;
        }


        private void btnConfermaPrenotazione_Click(object sender, RoutedEventArgs e)
        {
            if (dtpData.SelectedDate.HasValue)
            {
                Data = dtpData.SelectedDate.Value;
                Pasto = radPranzo.IsChecked == true ? "Pranzo" : "Cena";
                NumeroPersonePrenotanti = (int)sldNumeroPersone.Value;
                DialogResult = true; // Imposta il risultato della finestra di dialogo su "True" per confermare la selezione
            }
            else
            {
                _ = MessageBox.Show("Selezionare una data.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnnullaPrenotazione_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
