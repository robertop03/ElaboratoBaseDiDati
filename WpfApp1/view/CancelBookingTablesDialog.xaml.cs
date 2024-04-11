using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per CancelBookingTablesDialog.xaml
    /// </summary>
    public partial class CancelBookingTablesDialog : Window
    {
        public int IdTavolo { get; set; }
        public DateTime Data { get; private set; }
        public string Pasto { get; private set; }
        public bool Result { get; private set; }

        public CancelBookingTablesDialog(List<string> prenotazioni)
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
                    int startIndex = prenotazioneSelezionata.IndexOf("il ") + "il ".Length;
                    int endIndex = prenotazioneSelezionata.IndexOf(" a ") - 1;
                    string dateString = prenotazioneSelezionata.Substring(startIndex, endIndex - startIndex + 1);

                    Data = DateTime.Parse(dateString);
                    int startIndexPasto = prenotazioneSelezionata.IndexOf(" a ") + " a ".Length;
                    string pastoParte = prenotazioneSelezionata.Substring(startIndexPasto);
                    string[] partiPasto = pastoParte.Split(' ');
                    Pasto = partiPasto[0];
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
            Title = "Prenotazioni - Tavolo numero " + IdTavolo;
        }

        private void bntConfermaSelezione_Click(object sender, RoutedEventArgs e)
        {
            if (lstPrenotazioni.SelectedItem != null)
            {
                string prenotazioneSelezionata = (string)lstPrenotazioni.SelectedItem;
                int startIndex = prenotazioneSelezionata.IndexOf("il ") + "il ".Length;
                int endIndex = prenotazioneSelezionata.IndexOf(" a ") - 1;
                string dateString = prenotazioneSelezionata.Substring(startIndex, endIndex - startIndex + 1);

                Data = DateTime.Parse(dateString);
                int startIndexPasto = prenotazioneSelezionata.IndexOf(" a ") + " a ".Length;
                string pastoParte = prenotazioneSelezionata.Substring(startIndexPasto);
                string[] partiPasto = pastoParte.Split(' ');
                Pasto = partiPasto[0];
                Result = true;
                Close();
            }
            else
            {
                Result = false;
                _ = MessageBox.Show("Selezionare prima una prenotazione per la quale effettuare l'ordine.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
