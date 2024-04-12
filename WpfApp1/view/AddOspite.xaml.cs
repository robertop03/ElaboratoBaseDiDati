using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AddOspite.xaml
    /// </summary>
    public partial class AddOspite : Window
    {
        public AddOspite()
        {
            InitializeComponent();
        }

        public bool Result { get; private set; }
        public string CodiceFiscale { get; private set; }
        public string Nome { get; private set; }
        public string Cognome { get; private set; }
        public string NumeroTelefono { get; private set; }
        public string Nickname { get; private set; }

        private void btnConfermaOspite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codiceFiscale = txtCodiceFiscale.Text.Trim();
                string nome = txtNome.Text.Trim();
                string cognome = txtCognome.Text.Trim();
                string numeroTelefono = txtNumeroTelefono.Text.Trim();
                string nickname = txtNicknameOspite.Text.Trim();
                if (string.IsNullOrEmpty(codiceFiscale))
                {
                    _ = txtCodiceFiscale.Focus();
                    throw new ArgumentNullException("Codice fiscale non inserito.");
                }
                if (codiceFiscale.Length != 16)
                {
                    _ = txtCodiceFiscale.Focus();
                    txtCodiceFiscale.SelectAll();
                    throw new Exception("Il codice fiscale deve essere di 16 caratteri");
                }

                if (string.IsNullOrEmpty(nome))
                {
                    _ = txtNome.Focus();
                    throw new ArgumentNullException("Nome non inserito.");
                }
                if (nome.Length < 3)
                {
                    _ = txtNome.Focus();
                    txtNome.SelectAll();
                    throw new Exception("Il nome deve avere almeno 3 caratteri");
                }
                if (IsStringContainingNumber(nome))
                {
                    _ = txtNome.Focus();
                    txtNome.SelectAll();
                    throw new Exception("Il nome non può contenere caratteri numerici");
                }

                if (string.IsNullOrEmpty(cognome))
                {
                    _ = txtCognome.Focus();
                    throw new ArgumentNullException("Cognome non inserito.");
                }
                if (cognome.Length < 3)
                {
                    _ = txtCognome.Focus();
                    txtCognome.SelectAll();
                    throw new Exception("Il cognome deve avere almeno 3 caratteri");
                }
                if (IsStringContainingNumber(cognome))
                {
                    _ = txtCognome.Focus();
                    txtCognome.SelectAll();
                    throw new Exception("Il cognome non può contenere caratteri numerici");
                }

                if (string.IsNullOrEmpty(numeroTelefono))
                {
                    _ = txtNumeroTelefono.Focus();
                    throw new ArgumentNullException("Numero di telefono non inserito.");
                }
                if (numeroTelefono.Length != 10)
                {
                    _ = txtNumeroTelefono.Focus();
                    txtNumeroTelefono.SelectAll();
                    throw new Exception("Il numero di telefono deve essere di 10 caratteri");
                }
                if (!IsStringAllNumeric(numeroTelefono))
                {
                    _ = txtNumeroTelefono.Focus();
                    txtNumeroTelefono.SelectAll();
                    throw new Exception("Il numero di telefono deve contenere solo caratteri numerici");
                }

                if (string.IsNullOrEmpty(nickname))
                {
                    _ = txtNicknameOspite.Focus();
                    throw new ArgumentNullException("Nickname non inserito.");
                }
                if (nickname.Length < 2)
                {
                    _ = txtNicknameOspite.Focus();
                    txtNicknameOspite.SelectAll();
                    throw new Exception("Il nickname deve avere almeno 2 caratteri");
                }

                CodiceFiscale = codiceFiscale;
                Nome = nome;
                Cognome = cognome;
                NumeroTelefono = numeroTelefono;
                Nickname = nickname;

                Result = true;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnnullaOspite_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private bool IsStringAllNumeric(string str) => long.TryParse(str, out _);

        private bool IsStringContainingNumber(string str)
        {
            Regex regex = new Regex("[0-9]");
            return regex.IsMatch(str);
        }
    }
}
