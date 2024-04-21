using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per CancelBookingDialog.xaml
    /// </summary>
    public partial class CancelBookingDialog : Window
    {
        public int NumeroRigaOmbrellone { private get; set; }
        public int NumeroColonnaOmbrellone { private get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public bool Result { get; private set; }

        public CancelBookingDialog(List<string> prenotazioni)
        {
            InitializeComponent();
            lstPrenotazioni.ItemsSource = prenotazioni;
        }

        private void btnConfermaDisdetta_Click(object sender, RoutedEventArgs e)
        {
            if (lstPrenotazioni.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Sei sicuro di voler disdire questa prenotazione?", "Conferma Disdetta", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string prenotazioneSelezionata = (string)lstPrenotazioni.SelectedItem;
                    int startIndexPrimaData = prenotazioneSelezionata.IndexOf("da ") + "da ".Length;
                    int endIndexPrimaData = prenotazioneSelezionata.IndexOf(" a ") - 1;
                    string primaDataString = prenotazioneSelezionata.Substring(startIndexPrimaData, endIndexPrimaData - startIndexPrimaData + 1);

                    int startIndexSecondaData = prenotazioneSelezionata.IndexOf(" a ") + " a ".Length;
                    int endIndexSecondaData = prenotazioneSelezionata.LastIndexOf(" da ") - 1;
                    string secondaDataString = prenotazioneSelezionata.Substring(startIndexSecondaData, endIndexSecondaData - startIndexSecondaData + 1);
                    DataInizio = DateTime.Parse(primaDataString);
                    DataFine = DateTime.Parse(secondaDataString);
                    _ = MessageBox.Show("La prenotazione è stata disdetta con successo.", "Disdetta", MessageBoxButton.OK, MessageBoxImage.Information);
                    Result = true;
                    Close();
                }
                else
                {
                    _ = MessageBox.Show("La disdetta è stata annullata.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    Result = false;
                    Close();
                }
            }
            else
            {
                _ = MessageBox.Show("Selezionare prima una prenotazione da disdire", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Prenotazioni - Ombrellone fila " + NumeroRigaOmbrellone + ", colonna " + NumeroColonnaOmbrellone;
        }
    }
}
