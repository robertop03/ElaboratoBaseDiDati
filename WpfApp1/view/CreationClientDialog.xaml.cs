using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per CreationClientDialog.xaml
    /// </summary>
    public partial class CreationClientDialog : Window
    {
        public bool Result { get; private set; }
        public string CodiceFiscale { get; private set; }
        public string Nome { get; private set; }
        public string Cognome { get; private set; }
        public string NumeroTelefono { get; private set; }
        public string Città { get; private set; }
        public string Via { get; private set; }
        public int NumeroCivico { get; private set; }
        public string Email { get; private set; }
        public string CodiceDocumento { get; private set; }
        public string TipoDocumento { get; private set; }
        public int NumeroPersonePrenotati { get; private set; }

        public CreationClientDialog(DateTime dataInizio, DateTime dataFine)
        {
            InitializeComponent();
            _ = txtCodiceFiscale.Focus();
            TimeSpan durata = dataFine - dataInizio;
            if (durata.Days >= 30)
            {
                grbDocumento.Visibility = Visibility.Visible;
            }
            else
            {
                Height -= grbDocumento.Height;
                grbDocumento.Visibility = Visibility.Collapsed;
            }
        }

        private void btnConfermaCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codiceFiscale = txtCodiceFiscale.Text.Trim();
                string nome = txtNome.Text.Trim();
                string cognome = txtCognome.Text.Trim();
                string numeroTelefono = txtNumeroTelefono.Text.Trim();
                string città = txtCitta.Text.Trim();
                string via = txtVia.Text.Trim();
                string numeroCivico = txtNumeroCivico.Text.Trim();
                string email = txtEmail.Text.Trim();
                int numeroPersonePrenotati = (int)sldNumeroPersone.Value;
                string codiceDocumento = "", tipoDocumento = "";
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

                if (string.IsNullOrEmpty(città))
                {
                    _ = txtCitta.Focus();
                    throw new ArgumentNullException("Città non inserita.");
                }
                if (città.Length < 2)
                {
                    _ = txtCitta.Focus();
                    txtCitta.SelectAll();
                    throw new Exception("La città deve avere almeno 2 caratteri");
                }
                if (IsStringContainingNumber(città))
                {
                    _ = txtCitta.Focus();
                    txtCitta.SelectAll();
                    throw new Exception("La città non deve contenere caratteri numerici");
                }

                if (string.IsNullOrEmpty(via))
                {
                    _ = txtVia.Focus();
                    throw new ArgumentNullException("Via non inserita.");
                }
                if (via.Length < 3)
                {
                    _ = txtVia.Focus();
                    txtVia.SelectAll();
                    throw new Exception("La via deve contenere almeno 3 caratteri");
                }
                if (IsStringContainingNumber(via))
                {
                    _ = txtVia.Focus();
                    txtVia.SelectAll();
                    throw new Exception("La via non deve contenere caratteri numerici");
                }

                if (string.IsNullOrEmpty(numeroCivico))
                {
                    _ = txtNumeroCivico.Focus();
                    throw new ArgumentNullException("Numero civico non inserito.");
                }
                if (numeroCivico.Length > 4)
                {
                    _ = txtNumeroCivico.Focus();
                    txtNumeroCivico.SelectAll();
                    throw new Exception("Il numero civico non può avere più di 4 caratteri");
                }
                if (!IsStringAllNumeric(numeroCivico))
                {
                    _ = txtNumeroCivico.Focus();
                    txtNumeroCivico.SelectAll();
                    throw new Exception("Il numero civico deve contenere solo caratteri numerici");
                }

                if (cbxEmail.IsChecked.Value)
                {
                    if (string.IsNullOrEmpty(email))
                    {
                        _ = txtEmail.Focus();
                        throw new ArgumentNullException("Email non inserita.");
                    }
                    if (!IsValidEmail(email))
                    {
                        _ = txtEmail.Focus();
                        txtEmail.SelectAll();
                        throw new Exception("Formato della mail non valido.");
                    }
                }

                if (grbDocumento.Visibility == Visibility.Visible)
                {
                    codiceDocumento = txtCodiceDocumento.Text.Trim();
                    tipoDocumento = cmbTipoDocumento.SelectedItem.ToString();
                    if (string.IsNullOrEmpty(codiceDocumento))
                    {
                        throw new ArgumentNullException("Codice documento non inserito.");
                    }
                }

                CodiceFiscale = codiceFiscale;
                Nome = nome;
                Cognome = cognome;
                NumeroTelefono = numeroTelefono;
                Città = città;
                Via = via;
                NumeroCivico = int.Parse(numeroCivico);
                Email = email;
                CodiceDocumento = codiceDocumento;
                TipoDocumento = tipoDocumento;
                NumeroPersonePrenotati = numeroPersonePrenotati;

                Result = true;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnnullaCliente_Click(object sender, RoutedEventArgs e)
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

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtCodiceFiscale.Text = "RSSMRA80A01H501S";
            txtNome.Text = "Roberto";
            txtCognome.Text = "Pisu";
            txtNumeroTelefono.Text = "3921629515";
            txtVia.Text = "Leon Battista";
            txtNumeroCivico.Text = "4";
            txtCitta.Text = "Savignano";
        }
    }
}
