using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per SelectExistentClientDialog.xaml
    /// </summary>
    public partial class SelectExistentClientDialog : Window
    {
        public bool Result { get; private set; }
        public string CodiceFiscale { get; private set; }

        public SelectExistentClientDialog(List<string> clienti)
        {
            InitializeComponent();
            lstClienti.ItemsSource = clienti;
        }

        private void btnConfermaClienteSelezionato_Click(object sender, RoutedEventArgs e)
        {
            string pattern = @"Codice Fiscale: (\w+),";

            if (lstClienti.SelectedItem != null)
            {
                string clienteSelezionato = (string)lstClienti.SelectedItem;
                Match match = Regex.Match(clienteSelezionato, pattern);
                CodiceFiscale = match.Groups[1].Value;
                Result = true;
                Close();
            }
            else
            {
                _ = MessageBox.Show("Selezionare un cliente dalla lista.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnCreaNuovoCliente_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}
