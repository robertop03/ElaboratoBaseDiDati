using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per AddEvento_Ospite.xaml
    /// </summary>
    public partial class AddEvento_Ospite : Window
    {
        private readonly ControllerImpl controller;

        internal AddEvento_Ospite()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            controller.AggiuntoEvento += Controller_ModifiedEvents;
            controller.RimossoEvento += Controller_ModifiedEvents;
            controller.AggiuntoOspite += Controller_ModifiedOspiti;
            controller.RimossoOspite += Controller_ModifiedOspiti;
            lstEventi.ItemsSource = controller.GetEvents();
            lstOspiti.ItemsSource = controller.GetOspiti();
        }

        private void Controller_ModifiedEvents(object sender, EventArgs e)
        {
            lstEventi.ItemsSource = controller.GetEvents();
        }

        private void Controller_ModifiedOspiti(object sender, EventArgs e)
        {
            lstOspiti.ItemsSource = controller.GetOspiti();
        }

        private void btnAggiungiEvento_Click(object sender, RoutedEventArgs e)
        {
            List<string> ospiti = new List<string>();
            foreach (object selectedItem in lstOspiti.SelectedItems)
            {
                ospiti.Add(selectedItem.ToString());
            }

            AddEventoDialog addEventoDialog = new AddEventoDialog();
            _ = addEventoDialog.ShowDialog();
            if (addEventoDialog.Result)
            {
                controller.AggiungiEvento(addEventoDialog.Titolo, addEventoDialog.Data, addEventoDialog.Orario, addEventoDialog.Descrizione, addEventoDialog.CostoIngresso, ospiti);
                List<string> emails = controller.GetEmails();
                foreach (string email in emails)
                {

                }
                _ = MessageBox.Show("L'evento è stato aggiunto con successo e comunicato agli iscritti in newsletter.", "Evento aggiunto.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnRimuoviEvento_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventi.SelectedItem != null)
            {
                string eventoSelezionato = (string)lstEventi.SelectedItem;
                Regex titleRegex = new Regex(@"Evento: (?<title>[^,]+),");
                Match titleMatch = titleRegex.Match(eventoSelezionato);
                string titolo = titleMatch.Success ? titleMatch.Groups["title"].Value : string.Empty;

                Regex dateRegex = new Regex(@"Data: (?<date>[^,]+),");
                Match dateMatch = dateRegex.Match(eventoSelezionato);
                DateTime data = dateMatch.Success ? DateTime.Parse(dateMatch.Groups["date"].Value) : DateTime.MinValue;
                controller.RimuoviEvento(titolo, data);
            }
            else
            {
                _ = MessageBox.Show("Selezionare prima un evento da eliminare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnAggiungiOspite_Click(object sender, RoutedEventArgs e)
        {
            AddOspite addOspite = new AddOspite();
            _ = addOspite.ShowDialog();
            if (addOspite.Result)
            {
                controller.AggiungiOspite(addOspite.CodiceFiscale, addOspite.Cognome, addOspite.Nome, addOspite.NumeroTelefono, addOspite.Nickname);
            }
        }

        private void btnRimuoviOspite_Click(object sender, RoutedEventArgs e)
        {
            if (lstOspiti.SelectedItem != null)
            {
                string ospiteStr = (string)lstOspiti.SelectedItem;
                Regex regex = new Regex(@"CF: (?<cf>[^,]+)");
                Match cfMatch = regex.Match(ospiteStr);
                string cf = cfMatch.Success ? cfMatch.Groups["cf"].Value.Trim() : string.Empty;
                controller.RimuoviOspite(cf);
            }
            else
            {
                _ = MessageBox.Show("Selezionare prima un ospite da eliminare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
