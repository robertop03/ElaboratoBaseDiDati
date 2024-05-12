using System.Text.RegularExpressions;
using System.Windows;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AllDocuments.xaml
    /// </summary>
    public partial class AllDocuments : Window
    {
        private readonly ControllerImpl controller;
        public bool Result { get; private set; }
        public string CodiceDocumento { get; private set; }
        public string TipoDocumento { get; private set; }
        public string CodiceFiscaleCliente { get; private set; }

        public AllDocuments()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            lstDocumenti.ItemsSource = controller.GetDocumenti();
        }

        private void btnConfermaDocumentoSelezionato_Click(object sender, RoutedEventArgs e)
        {
            if (lstDocumenti.SelectedItem != null)
            {
                string documentoSelezionato = (string)lstDocumenti.SelectedItem;
                string pattern = @"Tipo: (?<tipo>\w+), Codice documento: (?<codiceDocumento>\w+), Cf cliente: (?<codiceFiscaleCliente>\w+)";
                Match match = Regex.Match(documentoSelezionato, pattern);
                if (match.Success)
                {
                    TipoDocumento = match.Groups["tipo"].Value;
                    CodiceDocumento = match.Groups["codiceDocumento"].Value;
                    CodiceFiscaleCliente = match.Groups["codiceFiscaleCliente"].Value;
                }
                Result = true;
                Close();
            }
            else
            {
                _ = MessageBox.Show("Selezionare prima un documento.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void bntAggiungiDocumento_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}
