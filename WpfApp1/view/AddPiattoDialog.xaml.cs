using System;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AddPiattoDialog.xaml
    /// </summary>
    public partial class AddPiattoDialog : Window
    {
        public bool Result { get; private set; }
        public string Nome { get; private set; }
        public string Descrizione { get; private set; }
        public double Prezzo { get; private set; }

        public AddPiattoDialog()
        {
            InitializeComponent();
            _ = txtNomePiatto.Focus();
        }

        private void btnConfermaAggiuntaPiatto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nome = txtNomePiatto.Text.Trim();
                string descrizione = txtDescrizionePiatto.Text.Trim();
                string prezzo = txtPrezzoPiatto.Text.Trim();
                if (string.IsNullOrEmpty(nome))
                {
                    _ = txtNomePiatto.Focus();
                    throw new ArgumentNullException("Nome del piatto non inserito.");
                }
                if (nome.Length < 3)
                {
                    _ = txtNomePiatto.Focus();
                    txtNomePiatto.SelectAll();
                    throw new Exception("Il nome del piatto deve avere almeno 3 caratteri.");
                }
                if (IsStringAllNumeric(nome))
                {
                    _ = txtNomePiatto.Focus();
                    txtNomePiatto.SelectAll();
                    throw new Exception("Il nome del piatto non può contenere solo numeri.");
                }

                if (string.IsNullOrEmpty(descrizione))
                {
                    _ = txtDescrizionePiatto.Focus();
                    throw new ArgumentNullException("Descrizione del piatto non inserita.");
                }
                if (IsStringAllNumeric(nome))
                {
                    _ = txtDescrizionePiatto.Focus();
                    txtDescrizionePiatto.SelectAll();
                    throw new Exception("La descrizione del piatto non può contenere solo numeri.");
                }

                if (string.IsNullOrEmpty(prezzo))
                {
                    _ = txtPrezzoPiatto.Focus();
                    throw new ArgumentNullException("Prezzo del piatto non inserito.");
                }
                if (!IsStringAllNumericAndBetweenRange(prezzo))
                {
                    _ = txtPrezzoPiatto.Focus();
                    txtPrezzoPiatto.SelectAll();
                    throw new ArgumentNullException("Il prezzo del piatto deve essere un numero maggiore di 0 e minore o uguale a 5000.");
                }

                Nome = nome;
                Prezzo = double.Parse(prezzo);
                Descrizione = descrizione;

                Result = true;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnnullaAggiuntaPiatto_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private bool IsStringAllNumeric(string str) => long.TryParse(str, out _);

        private bool IsStringAllNumericAndBetweenRange(string str)
        {
            return double.TryParse(str, out double numericValue) && numericValue > 0 && numericValue <= 5000;
        }
    }
}
