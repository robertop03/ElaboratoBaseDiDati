using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per WindowSpiaggia.xaml
    /// </summary>
    public partial class WindowSpiaggia : Window
    {
        private readonly ControllerImpl controller;
        private readonly Dictionary<(int, int), Point> umbrellasPositions = new Dictionary<(int, int), Point>();
        private int numeroRiga = 1, numeroColonna = 1;

        public WindowSpiaggia()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            DataContext = controller;
            controller.AggiuntoOmbrellone += Controller_ModifiedNumberOmbrellas;
            controller.RimossoOmbrellone += Controller_ModifiedNumberOmbrellas;
            lblNumeroOmbrelloni.Content = $"Numero ombrelloni: {controller.GetNumeroOmbrelloni()}";

            int year = DateTime.Now.Year;
            dtpCalendar.DisplayDateStart = new DateTime(year, 6, 1);
            dtpCalendar.DisplayDateEnd = new DateTime(year, 9, 30);
            dtpCalendar.SelectedDate = dtpCalendar.DisplayDateStart;
            controller.LoadOmbrelloniFromDB();
            foreach ((int, int) ombrellone in controller.GetOmbrelloni())
            {
                AggiungiOmbrelloneAlCanvas(ombrellone.Item1, ombrellone.Item2);
            }
            if (controller.GetOmbrelloni().Count > 0)
            {
                (int, int) numeroRigaEColonna = controller.RigaEColonna();
                numeroRiga = numeroRigaEColonna.Item1;
                numeroColonna = numeroRigaEColonna.Item2 + 1;
            }
        }

        private const double MinimumSpacing = 17; // Spazio minimo tra gli ombrelloni
        private const double EdgeSpacing = 20; // Spazio minimo dai bordi del Canvas
        private const double UmbrellaSize = 50;

        private double currentLeft = EdgeSpacing; // Posizione corrente sull'asse X
        private double currentTop = EdgeSpacing; // Posizione corrente sull'asse Y

        private void Controller_ModifiedNumberOmbrellas(object sender, EventArgs e)
        {
            // Aggiorna il contenuto della Label ogni volta che viene aggiunto un ombrellone
            lblNumeroOmbrelloni.Content = $"Numero ombrelloni: {controller.GetNumeroOmbrelloni()}";
        }

        private Point GetNewUmbrellaPosition()
        {
            // Calcola la posizione corrente dell'ultimo ombrellone aggiunto
            double currentLeft = EdgeSpacing + (numeroColonna - 1) * (UmbrellaSize + MinimumSpacing);
            double currentTop = EdgeSpacing + (numeroRiga - 1) * (UmbrellaSize + MinimumSpacing);

            // Verifica se l'ultima posizione è già occupata da un ombrellone
            while (umbrellasPositions.ContainsValue(new Point(currentLeft, currentTop)))
            {
                // Sposta la posizione verso destra
                currentLeft += UmbrellaSize + MinimumSpacing;
                if (currentLeft + UmbrellaSize + MinimumSpacing > spiaggiaCanvas.ActualWidth - EdgeSpacing)
                {
                    // Passa alla riga successiva
                    currentLeft = EdgeSpacing;
                    currentTop += UmbrellaSize + MinimumSpacing;
                }
            }

            // Memorizza la nuova posizione nella mappa
            umbrellasPositions[(numeroRiga, numeroColonna)] = new Point(currentLeft, currentTop);

            return new Point(currentLeft, currentTop);
        }

        private void AggiungiOmbrelloneAlCanvas(int riga, int colonna)
        {
            CheckBox newCheckBox = new CheckBox();
            Image newUmbrella = new Image
            {
                Source = new BitmapImage(new Uri("../resources/umbrella_icon.png", UriKind.Relative)),
                Width = 50,
                Height = 50
            };
            newCheckBox.Content = newUmbrella;
            // Imposta la posizione dell'ombrello
            double leftPosition = EdgeSpacing + (colonna - 1) * (UmbrellaSize + MinimumSpacing);
            double topPosition = EdgeSpacing + (riga - 1) * (UmbrellaSize + MinimumSpacing);
            Canvas.SetLeft(newCheckBox, leftPosition);
            Canvas.SetTop(newCheckBox, topPosition);

            (int numeroRiga, int numeroColonna) rigaEcolonna = (riga, colonna);
            newCheckBox.Tag = rigaEcolonna;
            _ = spiaggiaCanvas.Children.Add(newCheckBox); // Aggiungi l'ombrello al Canvas

            umbrellasPositions[(riga, colonna)] = new Point(leftPosition, topPosition);
        }

        private void spiaggiaCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int maxColumns = CalcolaMassimoNumeroColonne(spiaggiaCanvas.ActualWidth);


            if (currentLeft + UmbrellaSize + MinimumSpacing > spiaggiaCanvas.ActualWidth - EdgeSpacing ||
                currentTop + UmbrellaSize + MinimumSpacing > spiaggiaCanvas.ActualHeight - EdgeSpacing)
            {
                _ = MessageBox.Show("Non c'è abbastanza spazio per aggiungere un altro ombrellone.", "Spazio esaurito", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Esci dal metodo
            }

            MessageBoxResult result = MessageBox.Show("Sei sicuro di voler aggiungere un ombrellone?", "Conferma inserimento", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Crea un nuovo ombrellone

                CheckBox newCheckBox = new CheckBox();
                Image newUmbrella = new Image
                {
                    Source = new BitmapImage(new Uri("../resources/umbrella_icon.png", UriKind.Relative)),
                    Width = 50,
                    Height = 50
                };
                newCheckBox.Content = newUmbrella;
                // Imposta la posizione dell'ombrello
                Point newPosition = GetNewUmbrellaPosition();
                Canvas.SetLeft(newCheckBox, newPosition.X);
                Canvas.SetTop(newCheckBox, newPosition.Y);

                if (currentLeft + UmbrellaSize + MinimumSpacing > spiaggiaCanvas.ActualWidth - EdgeSpacing)
                {
                    // Passa alla riga successiva
                    currentLeft = EdgeSpacing;
                    currentTop += UmbrellaSize + MinimumSpacing;
                    // Incrementa il numero di riga e resetta il numero di colonna
                    numeroRiga++;
                    numeroColonna = 1;
                }
                // Controlla se non c'è abbastanza spazio sulla stessa riga
                if (numeroColonna > maxColumns)
                {
                    // Passa alla riga successiva
                    currentLeft = EdgeSpacing;
                    currentTop += UmbrellaSize + MinimumSpacing;
                    // Incrementa il numero di riga e resetta il numero di colonna
                    numeroRiga++;
                    numeroColonna = 1;
                }

                // Aggiunta dell'ombrellone nella lista di ombrelloni mediante il controller
                controller.AggiungiOmbrellone(numeroRiga, numeroColonna);
                (int numeroRiga, int numeroColonna) rigaEcolonna = (numeroRiga, numeroColonna);
                newCheckBox.Tag = rigaEcolonna;
                _ = spiaggiaCanvas.Children.Add(newCheckBox); // Aggiungi l'ombrello al Canvas

                numeroColonna++;
                // Verifica se è necessario passare alla riga successiva
                if (numeroColonna > maxColumns)
                {
                    // Passa alla riga successiva
                    currentTop += UmbrellaSize + MinimumSpacing;
                    currentLeft = EdgeSpacing; // reimposta la posizione sull'asse X per la nuova riga
                                               // Incrementa il numero di riga e resetta il numero di colonna
                    numeroRiga++;
                    numeroColonna = 1;
                }
                else
                {
                    // Aggiorna la posizione corrente per il prossimo ombrello sulla stessa riga
                    currentLeft += UmbrellaSize + MinimumSpacing;
                }
            }
        }

        private int CalcolaMassimoNumeroColonne(double canvasWidth)
        {
            double larghezzaDisponibile = canvasWidth - 2 * EdgeSpacing;
            int numeroColonne = (int)(larghezzaDisponibile / (UmbrellaSize + MinimumSpacing));
            return numeroColonne;
        }

        private void btnDisdiciOmbrellone_Click(object sender, RoutedEventArgs e)
        {
            if (spiaggiaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in spiaggiaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    if (item.Content is Image image)
                    {
                        if (item.IsChecked == true)
                        {
                            (int, int) rigaEColonna = (ValueTuple<int, int>)item.Tag;
                            List<string> prenotazioni = controller.GetPrenotazioniOmbrellone(rigaEColonna.Item1, rigaEColonna.Item2);
                            if (prenotazioni.Count != 0)
                            {
                                CancelBookingDialog cancelBookingDialog = new CancelBookingDialog(prenotazioni)
                                {
                                    NumeroRigaOmbrellone = rigaEColonna.Item1,
                                    NumeroColonnaOmbrellone = rigaEColonna.Item2
                                };
                                _ = cancelBookingDialog.ShowDialog();
                                if (cancelBookingDialog.Result)
                                {
                                    controller.DisdiciOmbrellone(cancelBookingDialog.DataInizio, cancelBookingDialog.DataFine, rigaEColonna.Item1, rigaEColonna.Item2);
                                    image.Source = new BitmapImage(new Uri("../resources/umbrella_icon.png", UriKind.Relative));
                                }
                            }
                            else
                            {
                                _ = MessageBox.Show("Non ci sono prenotazioni riguardanti l'ombrellone selezionato.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                    item.IsChecked = false;
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona l'ombrellone da disdire.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnPrenotaOmbrellone_Click(object sender, RoutedEventArgs e)
        {
            if (spiaggiaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in spiaggiaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    if (item.Content is Image image)
                    {
                        if (item.IsChecked == true)
                        {
                            (int, int) rigaEColonna = (ValueTuple<int, int>)item.Tag;
                            int riga = rigaEColonna.Item1;
                            int colonna = rigaEColonna.Item2;
                            SelectDateDialog dialog = new SelectDateDialog();
                            List<string> clienti = controller.GetClienti();
                            SelectExistentClientDialog selectExistentClientDialog = new SelectExistentClientDialog(clienti);
                            if (dialog.ShowDialog() == true)
                            {
                                DateTime dataInizio = dialog.DataInizio;
                                DateTime dataFine = dialog.DataFine;
                                CreationClientDialog creationClientDialog = new CreationClientDialog(dataInizio, dataFine);
                                if (controller.ControlloOmbrelloneLibero(riga, colonna, dataInizio, dataFine))
                                {
                                    bool existAClient = false;
                                    if (clienti.Count > 0)
                                    {
                                        existAClient = true;
                                        _ = selectExistentClientDialog.ShowDialog();
                                    }
                                    if (selectExistentClientDialog.Result && existAClient)
                                    {
                                        controller.PrenotaOmbrellone(riga, colonna, dataInizio, dataFine, selectExistentClientDialog.CodiceFiscale, dialog.NumeroLettiniAggiunti);
                                        image.Source = new BitmapImage(new Uri("../resources/umbrella_icon_booked.png", UriKind.Relative));
                                        _ = MessageBox.Show("Prenotazione avvenuta con successo.", "Prenotazione completata.", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        _ = creationClientDialog.ShowDialog();
                                        if (creationClientDialog.Result)
                                        {
                                            controller.AggiungiCliente(creationClientDialog.Nome, creationClientDialog.Cognome, creationClientDialog.NumeroTelefono, creationClientDialog.Città,
                                                creationClientDialog.Via, creationClientDialog.NumeroCivico, creationClientDialog.Email, creationClientDialog.CodiceDocumento, creationClientDialog.CodiceFiscale);
                                            controller.PrenotaOmbrellone(riga, colonna, dataInizio, dataFine, creationClientDialog.CodiceFiscale, dialog.NumeroLettiniAggiunti);
                                            image.Source = new BitmapImage(new Uri("../resources/umbrella_icon_booked.png", UriKind.Relative));
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
                                    _ = MessageBox.Show("L'ombrellone risulta essere già prenotato in quel periodo.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                    }
                    item.IsChecked = false;
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona l'ombrellone da prenotare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dtpCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dataSelezionata = dtpCalendar.SelectedDate.Value;
            List<(int, int)> ombrelloniPrenotati = controller.OmbrelloniPrenotati(dataSelezionata);
            foreach (CheckBox checkBox in spiaggiaCanvas.Children.OfType<CheckBox>())
            {
                (int numeroRiga, int numeroColonna) = ((int, int))checkBox.Tag;
                if (ombrelloniPrenotati.Contains((numeroRiga, numeroColonna)))
                {
                    if (checkBox.Content is Image image)
                    {
                        image.Source = new BitmapImage(new Uri("../resources/umbrella_icon_booked.png", UriKind.Relative));
                    }
                }
                else
                {
                    if (checkBox.Content is Image image)
                    {
                        image.Source = new BitmapImage(new Uri("../resources/umbrella_icon.png", UriKind.Relative));
                    }
                }
            }
        }

        private void btnInfoOmbrellone_Click(object sender, RoutedEventArgs e)
        {
            if (spiaggiaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in spiaggiaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    (int, int) rigaEColonna = (ValueTuple<int, int>)item.Tag;
                    List<string> info = controller.GetPrenotazioniOmbrellone(rigaEColonna.Item1, rigaEColonna.Item2);
                    string infoString = string.Join(Environment.NewLine, info);
                    if (info.Count == 0) { infoString = "L'ombrellone non risulta prenotato."; }
                    _ = MessageBox.Show(infoString, "Informazioni Ombrellone F: " + rigaEColonna.Item1 + ", C: " + rigaEColonna.Item2, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona l'ombrellone di cui avere le informazioni.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnImpostaPrezzi_Click(object sender, RoutedEventArgs e)
        {
            SetPricesDialog setPricesDialog = new SetPricesDialog();
            _ = setPricesDialog.ShowDialog();
            if (setPricesDialog.Result)
            {
                // TODO: Impostare i valori presi nel database
            }
        }

        private void btnRimuoviOmbrellone_Click(object sender, RoutedEventArgs e)
        {
            if (spiaggiaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in spiaggiaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    (int, int) rigaEColonna = (ValueTuple<int, int>)item.Tag;
                    // controller.RimuoviTuttePrenotazioniOmbrellone(rigaEColonna.Item1, rigaEColonna.Item2);
                    controller.RimuoviOmbrellone(rigaEColonna.Item1, rigaEColonna.Item2);
                    spiaggiaCanvas.Children.Remove(item);
                }
            }
            else
            {
                _ = MessageBox.Show("Seleziona l'ombrellone da rimuovere.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
