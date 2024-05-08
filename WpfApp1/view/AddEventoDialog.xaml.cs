using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AddEventoDialog.xaml
    /// </summary>
    public partial class AddEventoDialog : Window
    {
        public bool Result { get; private set; }
        public string Titolo { get; private set; }
        public DateTime Data { get; private set; }
        public TimeSpan Orario { get; private set; }
        public string Descrizione { get; private set; }
        public List<string> Ospiti { get; private set; }
        public double CostoIngresso { get; private set; }

        public AddEventoDialog()
        {
            InitializeComponent();
            InitializeTimeSlider();
            _ = txtTitoloEvento.Focus();
            int year = DateTime.Now.Year;
            dtpDataEvento.DisplayDateStart = new DateTime(year, 6, 1);
            dtpDataEvento.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpDataEvento.SelectedDate = dtpDataEvento.DisplayDateStart;
        }

        public void InitializeTimeSlider()
        {
            const int totalMinutesInDay = 24 * 60;
            slrTime.Minimum = 0;
            slrTime.Maximum = totalMinutesInDay;

            slrTime.TickFrequency = 30;

            slrTime.ValueChanged += slrTime_ValueChanged;
        }

        private void btnConfermaCreazioneEvento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string titolo = txtTitoloEvento.Text.Trim();
                string descrizione = txtDescrizioneEvento.Text.Trim();
                DateTime data;
                string orarioStringa = lblTime.Content.ToString();
                string costoIngresso = txtCostoIngressoEvento.Text.Trim();

                data = dtpDataEvento.SelectedDate ?? throw new Exception("Inserire una data per l'evento.");

                if (string.IsNullOrEmpty(titolo))
                {
                    _ = txtTitoloEvento.Focus();
                    throw new ArgumentNullException("Titolo evento non inserito.");
                }
                if (titolo.Length < 3)
                {
                    _ = txtTitoloEvento.Focus();
                    txtTitoloEvento.SelectAll();
                    throw new Exception("Il titolo dell'evento deve contenere almeno 3 caratteri.");
                }

                if (string.IsNullOrEmpty(descrizione))
                {
                    _ = txtDescrizioneEvento.Focus();
                    throw new ArgumentNullException("Descrizione evento non inserito.");
                }
                if (descrizione.Length < 5)
                {
                    _ = txtDescrizioneEvento.Focus();
                    txtDescrizioneEvento.SelectAll();
                    throw new Exception("La descrizione evento deve avere almeno 5 caratteri.");
                }
                TimeSpan orario = TimeSpan.Parse(orarioStringa);


                if (string.IsNullOrEmpty(costoIngresso))
                {
                    _ = txtCostoIngressoEvento.Focus();
                    throw new ArgumentNullException("Costo ingresso non inserito.");
                }
                if (!IsStringAllNumericAndBetweenRange(costoIngresso))
                {
                    _ = txtCostoIngressoEvento.Focus();
                    txtCostoIngressoEvento.SelectAll();
                    throw new Exception("Il costo ingresso deve essere compreso tra 0 e 1000.");
                }

                Descrizione = descrizione;
                Titolo = titolo;
                Data = data;
                CostoIngresso = double.Parse(costoIngresso);
                Orario = orario;

                Result = true;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnAnnullaCreazioneEvento_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private bool IsStringAllNumericAndBetweenRange(string str)
        {
            return double.TryParse(str, out double numericValue) && numericValue >= 0 && numericValue <= 1000;
        }

        private void slrTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan time = TimeSpan.FromMinutes(slrTime.Value);
            lblTime.Content = time.ToString(@"hh\:mm");
        }
    }
}
