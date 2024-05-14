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
        private readonly ControllerImpl controller;

        private readonly int idTavolo;
        private readonly DateTime data;
        private readonly string pasto;


        internal OrderDialog(ControllerImpl controller, DateTime? data, string pasto, int? idTavolo)
        {
            InitializeComponent();
            if (data.HasValue && idTavolo.HasValue)
            {
                this.idTavolo = idTavolo.Value;
                this.data = data.Value;
            }

            this.pasto = pasto;
            this.controller = controller;
            this.controller.AggiuntoPiatto += Controller_ModifiedPlates;
            this.controller.RimossoPiatto += Controller_ModifiedPlates;
            this.controller.AggiuntoMenu += Controller_ModifiedMenus;
            this.controller.RimossoMenu += Controller_ModifiedMenus;
            lstPiatti.ItemsSource = this.controller.GetPiatti();
            lstMenu.ItemsSource = this.controller.GetMenu();
            ControllerImpl.IdMenu = controller.GetLastMenuId() + 1;
            ControllerImpl.IdOrdine = controller.GetLastIdOrdine() + 1;
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
                    try
                    {
                        controller.AggiungiMenu(ControllerImpl.IdMenu++, addMenuDialog.Prezzo, piatti);
                        _ = MessageBox.Show("il menù è stato aggiunto con successo.", "Menù aggiunto.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

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
                try
                {
                    controller.AggiungiPiatto(addPiattoDialog.Nome, addPiattoDialog.Prezzo, addPiattoDialog.Descrizione);
                    _ = MessageBox.Show("il piatto è stato aggiunto con successo.", "Piatto aggiunto.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"Attenzione: {ex.Message}", "Piatto non aggiunto.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        private Dictionary<int, int> quantitaMenu = new Dictionary<int, int>();
        private Dictionary<string, int> quantitaPiatti = new Dictionary<string, int>();

        private void btnAggiungiAOrdine_Click(object sender, RoutedEventArgs e)
        {

            if (lstMenu.SelectedItem != null || lstPiatti.SelectedItem != null)
            {
                foreach (object selectedItem in lstMenu.SelectedItems)
                {
                    string menuSelezionato = (string)selectedItem;
                    Regex regex = new Regex(@"Menù n°: (\d+)");
                    Match match = regex.Match(menuSelezionato);
                    int idMenu = int.Parse(match.Groups[1].Value);

                    if (quantitaMenu.ContainsKey(idMenu))
                    {
                        quantitaMenu[idMenu]++;
                    }
                    else
                    {
                        quantitaMenu[idMenu] = 1;
                    }
                }

                foreach (object selectedItem in lstPiatti.SelectedItems)
                {
                    string piattoSelezionato = (string)selectedItem;
                    Regex regex = new Regex(@"Piatto: (.+?),");
                    Match match = regex.Match(piattoSelezionato);
                    string nome = match.Groups[1].Value;

                    if (quantitaPiatti.ContainsKey(nome))
                    {
                        quantitaPiatti[nome]++;
                    }
                    else
                    {
                        quantitaPiatti[nome] = 1;
                    }
                }
                _ = MessageBox.Show("Elementi aggiunti all'ordine.", "Ordine aggiornato.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _ = MessageBox.Show("Selezionare almeno un menù o un piatto.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnInviaOrdine_Click(object sender, RoutedEventArgs e)
        {
            List<int> idMenuOrdinati = new List<int>(quantitaMenu.Keys);
            List<string> piattiOrdinati = new List<string>(quantitaPiatti.Keys);
            try
            {
                controller.AggiungiOrdine(ControllerImpl.IdOrdine, data, pasto, idTavolo, idMenuOrdinati, piattiOrdinati);
                for (int i = 0; i < idMenuOrdinati.Count; i++)
                {
                    int quantita = quantitaMenu.ContainsKey(idMenuOrdinati[i]) ? quantitaMenu[idMenuOrdinati[i]] : 0;
                    controller.AggiungiMenuInContenenzaMenu(idMenuOrdinati[i], ControllerImpl.IdOrdine, quantita);
                }

                for (int i = 0; i < piattiOrdinati.Count; i++)
                {
                    int quantita = quantitaPiatti.ContainsKey(piattiOrdinati[i]) ? quantitaPiatti[piattiOrdinati[i]] : 0;
                    controller.AggiungiPiattiInContenenzaPiatti(piattiOrdinati[i], ControllerImpl.IdOrdine, quantita);
                }
                _ = MessageBox.Show("L'ordine è stato completato.", "Ordine inviato.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Attenzione: {ex.Message}", "Operazione annullata.", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Close();
        }
    }
}
