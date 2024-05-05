using System;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AddSconto.xaml
    /// </summary>
    public partial class AddSconto : Window
    {
        public bool Result { get; private set; }
        public int NumeroGiorni { get; private set; }
        public double ScontoPercentuale { get; private set; }

        public AddSconto()
        {
            InitializeComponent();
        }

        private void btnAnnullaSconto_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void btnConfermaSconto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string percentuale = txtPercentualeSconto.Text.Trim();

                if (string.IsNullOrEmpty(percentuale))
                {
                    _ = txtPercentualeSconto.Focus();
                    throw new ArgumentNullException("Percentuale di sconto non inserita.");
                }
                if (!IsStringAllNumericAndBetweenRange(percentuale))
                {
                    _ = txtPercentualeSconto.Focus();
                    txtPercentualeSconto.SelectAll();
                    throw new Exception("La percentuale di sconto deve essere compresa tra 0 e 100.");
                }

                ScontoPercentuale = double.Parse(percentuale);

                if (rdn15.IsChecked == true)
                {
                    NumeroGiorni = int.Parse(rdn15.Content.ToString());
                }
                else if (rdn30.IsChecked == true)
                {
                    NumeroGiorni = int.Parse(rdn30.Content.ToString());
                }

                Result = true;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsStringAllNumericAndBetweenRange(string str)
        {
            return double.TryParse(str, out double numericValue) && numericValue > 0 && numericValue <= 100;
        }
    }
}
