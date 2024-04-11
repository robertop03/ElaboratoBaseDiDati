using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per OrderDialog.xaml
    /// </summary>
    public partial class OrderDialog : Window
    {
        private ControllerImpl controller;

        private int idTavolo;
        private DateTime data;
        private string pasto;

        internal OrderDialog(ControllerImpl contollerPassed, DateTime? data, string pasto, int? idTavolo)
        {
            InitializeComponent();
            if (data.HasValue && idTavolo.HasValue)
            {
                this.idTavolo = idTavolo.Value;
                this.data = data.Value;
            }

            this.pasto = pasto;
            controller = contollerPassed;
            controller.AggiuntoPiatto += Controller_ModifiedPlates;
            controller.RimossoPiatto += Controller_ModifiedPlates;
            controller.AggiuntoMenu += Controller_ModifiedMenus;
            controller.RimossoMenu += Controller_ModifiedMenus;

            lstPiatti.ItemsSource = controller.GetPiatti();
            lstMenu.ItemsSource = controller.GetMenu();
        }

        private void Controller_ModifiedPlates(object sender, EventArgs e)
        {
            lstPiatti.ItemsSource = controller.GetPiatti();
        }

        private void Controller_ModifiedMenus(object sender, EventArgs e)
        {
            lstMenu.ItemsSource = controller.GetMenu();
        }

        private void btnAggiungiMenu_Click(object sender, RoutedEventArgs e)
        {
            if (lstPiatti.SelectedItems.Count >= 2)
            {
                // Faccio selezionare i piatti dall'elenco a destra (minimo 2), faccio inserire il prezzo e poi dico menù + id_menu creato
                AddMenuDialog addMenuDialog = new AddMenuDialog();
                _ = addMenuDialog.ShowDialog();
                if (addMenuDialog.Result)
                {

                    List<string> piatti = new List<string>();
                    foreach (object selectedItem in lstPiatti.SelectedItems)
                    {
                        piatti.Add(selectedItem.ToString());
                    }
                    controller.AggiungiMenu(ControllerImpl.IdMenu++, addMenuDialog.Prezzo, piatti);
                    _ = MessageBox.Show("il menù è stato aggiunto con successo.", "Menù aggiunto.", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            else
            {
                _ = MessageBox.Show("Selezionare almeno 2 piatti da mettere nel menù.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnAggiungiPiatto_Click(object sender, RoutedEventArgs e)
        {
            AddPiattoDialog addPiattoDialog = new AddPiattoDialog();
            _ = addPiattoDialog.ShowDialog();
            if (addPiattoDialog.Result)
            {
                controller.AggiungiPiatto(addPiattoDialog.Nome, addPiattoDialog.Prezzo, addPiattoDialog.Descrizione);
                _ = MessageBox.Show("il piatto è stato aggiunto con successo.", "Piatto aggiunto.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnRimuoviPiatto_Click(object sender, RoutedEventArgs e)
        {
            if (lstPiatti.SelectedItem != null)
            {
                string piattoSelezionato = (string)lstPiatti.SelectedItem;
                Regex regex = new Regex(@"Piatto: (.+?),");
                Match match = regex.Match(piattoSelezionato);
                string nome = match.Groups[1].Value;
                controller.RimuoviPiatto(nome);
            }
            else
            {
                _ = MessageBox.Show("Selezionare prima un piatto da eliminare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnRimuoviMenu_Click(object sender, RoutedEventArgs e)
        {
            if (lstMenu.SelectedItem != null)
            {
                string menuSelezionato = (string)lstMenu.SelectedItem;
                Regex regex = new Regex(@"Menù n°: (\d+)");
                Match match = regex.Match(menuSelezionato);
                int idMenu = int.Parse(match.Groups[1].Value);
                controller.RimuoviMenu(idMenu);
            }
            else
            {
                _ = MessageBox.Show("Selezionare prima un menù da eliminare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnOrdina_Click(object sender, RoutedEventArgs e)
        {
            List<int> menuOrdinati = new List<int>();
            List<string> piattiOrdinati = new List<string>();
            if (lstMenu.SelectedItem != null || lstPiatti.SelectedItem != null)
            {
                foreach (object selectedItem in lstMenu.SelectedItems)
                {
                    string menuSelezionato = (string)selectedItem;
                    Regex regex = new Regex(@"Menù n°: (\d+)");
                    Match match = regex.Match(menuSelezionato);
                    int idMenu = int.Parse(match.Groups[1].Value);
                    menuOrdinati.Add(idMenu);
                }
                foreach (object selectedItem in lstPiatti.SelectedItems)
                {
                    string piattoSelezionato = (string)selectedItem;
                    Regex regex = new Regex(@"Piatto: (.+?),");
                    Match match = regex.Match(piattoSelezionato);
                    string nome = match.Groups[1].Value;
                    piattiOrdinati.Add(nome);
                }


                controller.AggiungiOrdine(ControllerImpl.IdOrdine++, data, pasto, idTavolo, menuOrdinati, piattiOrdinati);
                _ = MessageBox.Show("Ordine creato con successo.", "Ordine mandato.", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                _ = MessageBox.Show("Selezionare almeno un menù o un piatto.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
