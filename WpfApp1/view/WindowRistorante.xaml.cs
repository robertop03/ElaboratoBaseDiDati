using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per WindowRistorante.xaml
    /// </summary>
    public partial class WindowRistorante : Window
    {
        private readonly ControllerImpl controller;
        private int idTavolo = 1; // In realtà qui non parto per forza da 1 ma vado a vedere nel db l'ultimo tavolo che id ha


        public WindowRistorante()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            DataContext = controller;
            controller.AggiuntoTavolo += Controller_ModifiedNumberTables;
            controller.RimossoTavolo += Controller_ModifiedNumberTables;
            lblNumeroTavoli.Content = $"Numero tavoli: {controller.GetNumeroTavoli()}";
            if (controller.GetNumeroTavoli() > 0)
            {
                idTavolo = controller.GetLastIdTavolo() + 1;
            }

            int year = DateTime.Now.Year;
            dtpCalendar.DisplayDateStart = new DateTime(year, 6, 1);
            dtpCalendar.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpCalendar.SelectedDate = dtpCalendar.DisplayDateStart;

            controller.LoadTavoliFromDB();
            salaCanvas.Loaded += SalaCanvas_Loaded;
        }


        private const double MinimumSpacing = 20;
        private const double EdgeSpacing = 20; // Spazio minimo dai bordi del Canvas
        private const double TableSize = 50;

        private double currentLeft = EdgeSpacing; // Posizione corrente sull'asse X
        private double currentTop = EdgeSpacing; // Posizione corrente sull'asse Y

        private void SalaCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            // Il canvas è stato completamente caricato, puoi ora ottenere le sue dimensioni
            foreach (int tavolo in controller.GetTavoli())
            {
                AggiungiTavoloAlCanvas(tavolo);
            }
        }

        private void Controller_ModifiedNumberTables(object sender, EventArgs e)
        {
            // Aggiorna il contenuto della Label ogni volta che viene aggiunto un tavolo
            lblNumeroTavoli.Content = "Numero tavoli: " + controller.GetNumeroTavoli();
        }

        private void AggiungiTavoloAlCanvas(int idTavolo)
        {
            CheckBox newCheckBox = new CheckBox();
            Image newTable = new Image
            {
                Source = new BitmapImage(new Uri("../resources/table_icon.png", UriKind.Relative)),
                Width = 50,
                Height = 50
            };
            newCheckBox.Content = newTable;

            // Imposta la posizione del tavolo basata sulla dimensione del canvas e sulla posizione corrente

            // Aggiungi il tavolo al canvas
            Canvas.SetLeft(newCheckBox, currentLeft);
            Canvas.SetTop(newCheckBox, currentTop);
            _ = salaCanvas.Children.Add(newCheckBox);

            TextBlock seatsTextBlock = new TextBlock
            {
                Text = controller.GetNumeroPostiTavolo(idTavolo).ToString(),
                FontSize = 10,
                Foreground = Brushes.Black
            };

            // Imposta la posizione del blocco di testo per i posti
            Canvas.SetLeft(seatsTextBlock, currentLeft + TableSize - seatsTextBlock.ActualWidth);
            Canvas.SetTop(seatsTextBlock, currentTop + TableSize - seatsTextBlock.ActualHeight);
            _ = salaCanvas.Children.Add(seatsTextBlock);

            newCheckBox.Tag = idTavolo;
            seatsTextBlock.Tag = idTavolo;

            // Aggiorna la posizione corrente per il prossimo tavolo
            currentLeft += TableSize + MinimumSpacing;

            // Controlla se il tavolo è l'ultimo nella riga corrente
            if (currentLeft + TableSize + MinimumSpacing > salaCanvas.ActualWidth - EdgeSpacing)
            {
                // Se è l'ultimo nella riga, sposta il prossimo tavolo nella riga successiva
                currentLeft = EdgeSpacing;
                currentTop += TableSize + MinimumSpacing;
            }
        }



        private void salaCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentLeft + TableSize + MinimumSpacing > salaCanvas.ActualWidth - EdgeSpacing ||
                currentTop + TableSize + MinimumSpacing > salaCanvas.ActualHeight - EdgeSpacing)
            {
                _ = MessageBox.Show("Non c'è abbastanza spazio per aggiungere un altro tavolo.", "Spazio esaurito", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Esci dal metodo
            }
            CustomMessageBoxAddTable selectSeatsDialog = new CustomMessageBoxAddTable();
            _ = selectSeatsDialog.ShowDialog();

            // Verifica della risposta dell'utente
            if (selectSeatsDialog.Result)
            {
                CheckBox newCheckBox = new CheckBox();
                Image newTable = new Image
                {
                    Source = new BitmapImage(new Uri("../resources/table_icon.png", UriKind.Relative)),
                    Width = 50,
                    Height = 50
                };
                newCheckBox.Content = newTable;
                Canvas.SetLeft(newCheckBox, currentLeft);
                Canvas.SetTop(newCheckBox, currentTop);
                _ = salaCanvas.Children.Add(newCheckBox);
                TextBlock seatsTextBlock = new TextBlock
                {
                    Text = selectSeatsDialog.SelectedSeats.ToString(),
                    FontSize = 10,
                    Foreground = Brushes.Black
                };

                Canvas.SetLeft(seatsTextBlock, currentLeft + TableSize - seatsTextBlock.ActualWidth);
                Canvas.SetTop(seatsTextBlock, currentTop + TableSize - seatsTextBlock.ActualHeight);
                _ = salaCanvas.Children.Add(seatsTextBlock);

                newCheckBox.Tag = idTavolo;
                seatsTextBlock.Tag = idTavolo;
                controller.AggiungiTavolo(idTavolo++, selectSeatsDialog.SelectedSeats);

                currentLeft += TableSize + MinimumSpacing;
                if (currentLeft + TableSize > salaCanvas.ActualWidth - EdgeSpacing)
                {
                    currentLeft = EdgeSpacing;
                    currentTop += TableSize + MinimumSpacing;
                }
            }
        }

        private void btnRimuoviTavolo_Click(object sender, RoutedEventArgs e)
        {
            if (salaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in salaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    TextBlock textBlock = salaCanvas.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Tag != null && (int)tb.Tag == (int)item.Tag);
                    if (textBlock != null)
                    {
                        salaCanvas.Children.Remove(textBlock);
                    }
                    // controller.RimuoviTuttePrenotazioniTavolo((int)item.Tag); NON TOGLIERE IL COMMENTO, SE RIMUOVO UN OMBRELLONE VOGLIO COMUNQUE CALCOLARE I GUARDAGNI
                    controller.RimuoviTavolo((int)item.Tag);
                    salaCanvas.Children.Remove(item);
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona il tavolo da rimuovere.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDisdiciTavolo_Click(object sender, RoutedEventArgs e)
        {
            if (salaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in salaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    if (item.Content is Image image)
                    {
                        if (item.IsChecked == true)
                        {
                            int idTavolo = (int)item.Tag;
                            List<string> prenotazioni = controller.GetPrenotazioniTavolo(idTavolo);
                            if (prenotazioni.Count != 0)
                            {
                                CancelBookingTablesDialog cancelBookingTablesDialog = new CancelBookingTablesDialog(prenotazioni)
                                {
                                    IdTavolo = idTavolo
                                };

                                cancelBookingTablesDialog.lblConfermaSelezionePerOrdine.Visibility = Visibility.Collapsed;
                                cancelBookingTablesDialog.btnConfermaSelezione.Visibility = Visibility.Collapsed;

                                _ = cancelBookingTablesDialog.ShowDialog();
                                if (cancelBookingTablesDialog.Result)
                                {
                                    controller.DisdiciTavolo(cancelBookingTablesDialog.IdTavolo, cancelBookingTablesDialog.Data, cancelBookingTablesDialog.Pasto);
                                    image.Source = new BitmapImage(new Uri("../resources/table_icon.png", UriKind.Relative));
                                }
                            }
                            else
                            {
                                _ = MessageBox.Show("Non ci sono prenotazioni riguardanti il tavolo selezionato.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                    item.IsChecked = false;
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona il tavolo da disdire.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnPrenotaTavolo_Click(object sender, RoutedEventArgs e)
        {
            if (salaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in salaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    if (item.Content is Image image)
                    {
                        if (item.IsChecked == true)
                        {
                            SelectSingolDate selectSingolDateDialog = new SelectSingolDate();
                            if (selectSingolDateDialog.ShowDialog() == true)
                            {
                                DateTime data = selectSingolDateDialog.Data;
                                string pasto = selectSingolDateDialog.Pasto;
                                List<string> clienti = controller.GetClienti();
                                SelectExistentClientDialog selectExistentClientDialog = new SelectExistentClientDialog(clienti);
                                CreationClientDialog creationClientDialog = new CreationClientDialog();
                                int idTavolo = (int)item.Tag;
                                if (controller.NumeroPostiTavoloAdegueato(idTavolo, selectSingolDateDialog.NumeroPersonePrenotanti))
                                {
                                    if (controller.ControlloTavoloLibero(idTavolo, data, pasto))
                                    {
                                        bool existAClient = false;
                                        if (clienti.Count > 0)
                                        {
                                            existAClient = true;
                                            _ = selectExistentClientDialog.ShowDialog();
                                        }
                                        if (selectExistentClientDialog.Result && existAClient)
                                        {
                                            controller.PrenotaTavolo((int)item.Tag, data, pasto, selectExistentClientDialog.CodiceFiscale, selectSingolDateDialog.NumeroPersonePrenotanti);
                                            image.Source = new BitmapImage(new Uri("../resources/table_icon_booked.png", UriKind.Relative));
                                            _ = MessageBox.Show("Prenotazione avvenuta con successo.", "Prenotazione completata.", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        else
                                        {
                                            _ = creationClientDialog.ShowDialog();
                                            if (creationClientDialog.Result)
                                            {
                                                controller.AggiungiCliente(creationClientDialog.Nome, creationClientDialog.Cognome, creationClientDialog.NumeroTelefono, creationClientDialog.Città,
                                                creationClientDialog.Via, creationClientDialog.NumeroCivico, creationClientDialog.Email, creationClientDialog.CodiceDocumento, creationClientDialog.CodiceFiscale);
                                                controller.PrenotaTavolo((int)item.Tag, data, pasto, creationClientDialog.CodiceFiscale, selectSingolDateDialog.NumeroPersonePrenotanti);
                                                image.Source = new BitmapImage(new Uri("../resources/table_icon_booked.png", UriKind.Relative));
                                                _ = MessageBox.Show("Registrazione avvenuta con successo.", "Registrazione completata.", MessageBoxButton.OK, MessageBoxImage.Information);
                                            }
                                            else
                                            {
                                                _ = MessageBox.Show("Registrazione annullata.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        _ = MessageBox.Show("Il tavolo risulta essere già prenotato in quel periodo.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    _ = MessageBox.Show("Il tavolo non contiene posti sufficienti per il numero di persone.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                        }
                    }
                    item.IsChecked = false;
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona il tavolo da prenotare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnInfoTavolo_Click(object sender, RoutedEventArgs e)
        {
            if (salaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in salaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    int idTavolo = (int)item.Tag;
                    List<string> info = controller.GetPrenotazioniTavolo(idTavolo);
                    string infoString = string.Join(Environment.NewLine, info);
                    if (info.Count == 0) { infoString = "Il tavolo non risulta prenotato."; }
                    _ = MessageBox.Show(infoString, "Informazioni Tavolo " + idTavolo, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona il tavolo di cui avere le informazioni.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOrdina_Click(object sender, RoutedEventArgs e)
        {
            if (salaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in salaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    int idTavolo = (int)item.Tag;
                    List<string> prenotazioni = controller.GetPrenotazioniTavolo(idTavolo);
                    if (prenotazioni.Count != 0)
                    {
                        CancelBookingTablesDialog cancelBookingTablesDialog = new CancelBookingTablesDialog(prenotazioni)
                        {
                            IdTavolo = idTavolo
                        };

                        cancelBookingTablesDialog.lblConfermaSelezionePerCancellazione.Visibility = Visibility.Collapsed;
                        cancelBookingTablesDialog.btnConfermaDisdetta.Visibility = Visibility.Collapsed;

                        _ = cancelBookingTablesDialog.ShowDialog();

                        OrderDialog orderDialog = new OrderDialog(controller, cancelBookingTablesDialog.Data, cancelBookingTablesDialog.Pasto, cancelBookingTablesDialog.IdTavolo);
                        orderDialog.btnAggiungiMenu.Visibility = Visibility.Collapsed;
                        orderDialog.btnRimuoviMenu.Visibility = Visibility.Collapsed;
                        orderDialog.btnAggiungiPiatto.Visibility = Visibility.Collapsed;
                        orderDialog.btnRimuoviPiatto.Visibility = Visibility.Collapsed;
                        _ = orderDialog.ShowDialog();
                    }
                    else
                    {
                        _ = MessageBox.Show("Non ci sono prenotazioni riguardanti il tavolo selezionato.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }


                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona il tavolo per il quale effettuare l'ordine.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void btnManageMenu_Click(object sender, RoutedEventArgs e)
        {
            OrderDialog orderDialog = new OrderDialog(controller, null, null, null);
            orderDialog.btnOrdina.Visibility = Visibility.Collapsed;
            _ = orderDialog.ShowDialog();
        }

        private void dtpCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dataSelezionata = dtpCalendar.SelectedDate.Value;
            List<int> tavoliPrenotati = controller.TavoliPrenotati(dataSelezionata);
            foreach (CheckBox checkBox in salaCanvas.Children.OfType<CheckBox>())
            {
                int idTavolo = (int)checkBox.Tag;
                if (tavoliPrenotati.Contains(idTavolo))
                {
                    if (checkBox.Content is Image image)
                    {
                        image.Source = new BitmapImage(new Uri("../resources/table_icon_booked.png", UriKind.Relative));
                    }
                }
                else
                {
                    if (checkBox.Content is Image image)
                    {
                        image.Source = new BitmapImage(new Uri("../resources/table_icon.png", UriKind.Relative));
                    }
                }
            }
        }
    }
}
