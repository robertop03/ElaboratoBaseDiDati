using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per WindowBilanci.xaml
    /// </summary>
    public partial class WindowBilanci : Window
    {
        private readonly ControllerImpl controller;

        public WindowBilanci()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            int year = DateTime.Now.Year;
            dtpDataInizio.DisplayDateStart = new DateTime(year, 6, 1);
            dtpDataInizio.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpDataInizio.SelectedDate = dtpDataInizio.DisplayDateStart;

            dtpDataFine.DisplayDateStart = new DateTime(year, 6, 1);
            dtpDataFine.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpDataFine.SelectedDate = dtpDataFine.DisplayDateStart;

            dtpDataInizioIncassi.DisplayDateStart = new DateTime(year, 6, 1);
            dtpDataInizioIncassi.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpDataInizioIncassi.SelectedDate = dtpDataInizioIncassi.DisplayDateStart;

            dtpDataFineIncassi.DisplayDateStart = new DateTime(year, 6, 1);
            dtpDataFineIncassi.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpDataFineIncassi.SelectedDate = dtpDataFineIncassi.DisplayDateStart;
        }

        private void btnCalcola_Click(object sender, RoutedEventArgs e)
        {
            lstResult.Items.Clear();
            ComboBoxItem selectedPiattoMenu = (ComboBoxItem)cmbPiattoMenu.SelectedItem;
            string stringPiattoMenu = selectedPiattoMenu.Content.ToString();
            ComboBoxItem selectedSceltoPiuOMeno = (ComboBoxItem)cmbPiuMeno.SelectedItem;
            string stringSceltoPiuOMeno = selectedSceltoPiuOMeno.Content.ToString();

            if (stringPiattoMenu.Equals("Piatto"))
            {
                if (stringSceltoPiuOMeno.Equals("Scelto meno"))
                {
                    (string, int) result = controller.GetIdPiattoMenoOrdinato(dtpDataInizio.SelectedDate.Value, dtpDataFine.SelectedDate.Value);
                    _ = lstResult.Items.Add($"Piatto: {result.Item1}, quantità: {result.Item2}");
                }
                else if (stringSceltoPiuOMeno.Equals("Scelto di più"))
                {
                    (string, int) result = controller.GetIdPiattoPiuOrdinato(dtpDataInizio.SelectedDate.Value, dtpDataFine.SelectedDate.Value);
                    _ = lstResult.Items.Add($"Piatto: {result.Item1}, quantità: {result.Item2}");
                }
            }
            else if (stringPiattoMenu.Equals("Menù"))
            {
                if (stringSceltoPiuOMeno.Equals("Scelto meno"))
                {
                    (int, int) result = controller.GetIdMenuMenoOrdinato(dtpDataInizio.SelectedDate.Value, dtpDataFine.SelectedDate.Value);
                    _ = lstResult.Items.Add($"Id menù: {result.Item1}, quantità: {result.Item2}");
                }
                else if (stringSceltoPiuOMeno.Equals("Scelto di più"))
                {
                    (int, int) result = controller.GetIdMenuPiuOrdinato(dtpDataInizio.SelectedDate.Value, dtpDataFine.SelectedDate.Value);
                    _ = lstResult.Items.Add($"Id menù: {result.Item1}, quantità: {result.Item2}");
                }
            }
        }

        private void btnPercentualeClientiConMail_Click(object sender, RoutedEventArgs e)
        {
            lblPercentuale.Content = $"{controller.CalcolaPercentualeClientiConMail():0.00} %";
        }

        private void btnCalcolaIncassi_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedSpiaggiaRistorante = (ComboBoxItem)cmbSpiaggiaRistorante.SelectedItem;
            string stringSpiaggiaRistorante = selectedSpiaggiaRistorante.Content.ToString();
            double incasso = 0;
            if (stringSpiaggiaRistorante.Equals("Spiaggia"))
            {
                incasso = controller.CalcolaIncassiSpiaggia(dtpDataInizioIncassi.SelectedDate.Value, dtpDataFineIncassi.SelectedDate.Value);
            }
            else if (stringSpiaggiaRistorante.Equals("Ristorante"))
            {
                incasso = controller.CalcolaIncassiRistorante(dtpDataInizioIncassi.SelectedDate.Value, dtpDataFineIncassi.SelectedDate.Value);
            }
            lblIncassi.Content = $"{incasso:0.00} €";
        }
    }
}
