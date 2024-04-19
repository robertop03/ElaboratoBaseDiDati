using System;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per SelectDateDialog.xaml
    /// </summary>
    public partial class SelectDateDialog : Window
    {
        public DateTime DataInizio { get; private set; }
        public DateTime DataFine { get; private set; }
        public int NumeroLettiniAggiunti { get; private set; }

        public SelectDateDialog()
        {
            InitializeComponent();
            int year = DateTime.Now.Year;
            dtpInizio.DisplayDateStart = new DateTime(year, 6, 1);
            dtpInizio.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpFine.DisplayDateStart = new DateTime(year, 6, 1);
            dtpFine.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpInizio.SelectedDate = dtpInizio.DisplayDateStart;
            dtpFine.SelectedDate = dtpFine.DisplayDateStart;
        }

        private void btnConferma_Click(object sender, RoutedEventArgs e)
        {
            if (dtpInizio.SelectedDate.HasValue && dtpFine.SelectedDate.HasValue)
            {
                if (dtpInizio.SelectedDate <= dtpFine.SelectedDate)
                {
                    int numeroLettiniAggiunti = 0;
                    DataInizio = dtpInizio.SelectedDate.Value;
                    DataFine = dtpFine.SelectedDate.Value;
                    if (rdnUno.IsChecked == true)
                    {
                        numeroLettiniAggiunti = 1;
                    }
                    else if (rdnDue.IsChecked == true)
                    {
                        numeroLettiniAggiunti = 2;
                    }
                    NumeroLettiniAggiunti = numeroLettiniAggiunti;
                    DialogResult = true;
                }
                else
                {
                    _ = MessageBox.Show("La data di inizio deve essere precedente o uguale alla data di fine.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Selezionare una data di inizio e una data di fine.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
