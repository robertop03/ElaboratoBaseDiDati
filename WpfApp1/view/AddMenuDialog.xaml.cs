using System;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AddMenuDialog.xaml
    /// </summary>
    public partial class AddMenuDialog : Window
    {
        public bool Result { get; private set; }
        public double Prezzo { get; private set; }

        public AddMenuDialog()
        {
            InitializeComponent();
        }

        private void btnConfermaCreazioneMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string prezzo = txtPrezzoMenu.Text.Trim();

                if (string.IsNullOrEmpty(prezzo))
                {
                    _ = txtPrezzoMenu.Focus();
                    throw new ArgumentNullException("Prezzo del menù non inserito.");
                }
                if (!IsStringAllNumericAndBetweenRange(prezzo))
                {
                    _ = txtPrezzoMenu.Focus();
                    txtPrezzoMenu.SelectAll();
                    throw new Exception("Il prezzo del menù deve essere un numero maggiore di 0 e minore o uguale a 5000.");
                }

                Prezzo = double.Parse(prezzo);

                Result = true;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnnullaCreazioneMenu_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }


        private bool IsStringAllNumericAndBetweenRange(string str)
        {
            return double.TryParse(str, out double numericValue) && numericValue > 0 && numericValue <= 5000;
        }
    }
}
