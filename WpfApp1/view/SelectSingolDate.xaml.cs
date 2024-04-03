using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per SelectSingolDate.xaml
    /// </summary>
    public partial class SelectSingolDate : Window
    {
        public DateTime Data { get; set; }
        public string Pasto { get; set; }

        public SelectSingolDate()
        {
            InitializeComponent();
            int year = DateTime.Now.Year;
            dtpData.DisplayDateStart = new DateTime(year, 6, 1);
            dtpData.DisplayDateEnd = new DateTime(year, 9, 30);
        }
        

        private void btnConfermaPrenotazione_Click(object sender, RoutedEventArgs e)
        {
            if (dtpData.SelectedDate.HasValue)
            {
                Data = dtpData.SelectedDate.Value;
                Pasto = radPranzo.IsChecked == true ? "Pranzo" : "Cena";
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
