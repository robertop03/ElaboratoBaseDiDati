using System;
using System.Text.RegularExpressions;
using System.Windows;
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
            controller.LoadScontiFromDB();
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
                try
                {
                    controller.AggiungiSconto(addSconto.NumeroGiorni, addSconto.ScontoPercentuale);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"Attenzione: {ex.Message}", "Operazione annullata", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                _ = MessageBox.Show("Sconto non aggiunto", "Operazione annullata", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
