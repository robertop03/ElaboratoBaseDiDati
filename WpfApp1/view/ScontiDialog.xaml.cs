using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per ScontiDialog.xaml
    /// </summary>
    public partial class ScontiDialog : Window
    {
        private readonly ControllerImpl controller;
        public bool Result { get; private set; }
        public ScontiDialog()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            controller.AggiuntoSconto += Controller_ModifiedSconti;
            controller.RimossoSconto += Controller_ModifiedSconti;
            lstSconti.ItemsSource = controller.GetSconti();
        }

        private void Controller_ModifiedSconti(object sender, EventArgs e)
        {
            lstSconti.ItemsSource = controller.GetSconti();
        }

        private void btnCancellaSconto_Click(object sender, RoutedEventArgs e)
        {
            if (lstSconti.SelectedItem != null)
            {
                Match numeroGiorniMatch = Regex.Match(lstSconti.SelectedItem.ToString(), @"Numero giorni: (\d+)");
                int numeroGiorni = numeroGiorniMatch.Success ? int.Parse(numeroGiorniMatch.Groups[1].Value) : -1;
                controller.RimuoviSconto(numeroGiorni);
            }
            else
            {
                _ = MessageBox.Show("Selezionare prima uno sconto da cancellare", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnAggiungiSconto_Click(object sender, RoutedEventArgs e)
        {
            AddSconto addSconto = new AddSconto();
            _ = addSconto.ShowDialog();
            if (addSconto.Result)
            {
                controller.AggiungiSconto(addSconto.NumeroGiorni, addSconto.ScontoPercentuale);
            }
            else
            {
                _ = MessageBox.Show("Sconto non aggiunto", "Operazione annullata", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
