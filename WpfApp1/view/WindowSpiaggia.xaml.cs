using System;
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
        private int numeroRiga = 1, numeroColonna = 1;

        public WindowSpiaggia()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            DataContext = controller;
            controller.AggiuntoOmbrellone += Controller_ModifiedNumberOmbrellas;
            controller.RimossoOmbrellone += Controller_ModifiedNumberOmbrellas;
            lblNumeroOmbrelloni.Content = "Numero ombrelloni: 0";

            int year = DateTime.Now.Year;
            dtpCalendar.DisplayDateStart = new DateTime(year, 6, 1);
            dtpCalendar.DisplayDateEnd = new DateTime(year, 9, 30);
        }

        private const double MinimumSpacing = 17; // Spazio minimo tra gli ombrelloni
        private const double EdgeSpacing = 20; // Spazio minimo dai bordi del Canvas
        private const double UmbrellaSize = 50;

        private double currentLeft = EdgeSpacing; // Posizione corrente sull'asse X
        private double currentTop = EdgeSpacing; // Posizione corrente sull'asse Y

        private void Controller_ModifiedNumberOmbrellas(object sender, EventArgs e)
        {
            // Aggiorna il contenuto della Label ogni volta che viene aggiunto un ombrellone
            lblNumeroOmbrelloni.Content = "Numero ombrelloni: " + controller.GetNumeroOmbrelloni();
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
            MessageBoxResult result = MessageBox.Show("Vuoi davvero aggiungere un ombrellone?", "Conferma", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Verifica della risposta dell'utente
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
                Canvas.SetLeft(newCheckBox, currentLeft);
                Canvas.SetTop(newCheckBox, currentTop);

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
                            controller.DisdiciOmbrellone(rigaEColonna.Item1, rigaEColonna.Item2);
                            image.Source = new BitmapImage(new Uri("../resources/umbrella_icon.png", UriKind.Relative));
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
                            // mostrare la cosa per decidere la durata della prenotazione
                            (int, int) rigaEColonna = (ValueTuple<int, int>)item.Tag;
                            int riga = rigaEColonna.Item1;
                            int colonna = rigaEColonna.Item2;
                            SelectDateDialog dialog = new SelectDateDialog();
                            
                            if (dialog.ShowDialog() == true)
                            {
                                DateTime dataInizio = dialog.DataInizio;
                                DateTime dataFine = dialog.DataFine;
                                if (controller.ControlloOmbrelloneLibero(riga, colonna, dataInizio, dataFine))
                                {
                                    controller.PrenotaOmbrellone(riga, colonna, dataInizio, dataFine);
                                    image.Source = new BitmapImage(new Uri("../resources/umbrella_icon_booked.png", UriKind.Relative));
                                }
                                else
                                {
                                    _ = MessageBox.Show("Stai tentando di prenotare un ombrellone che risulta già prenotato quel periodo.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnRimuoviOmbrellone_Click(object sender, RoutedEventArgs e)
        {
            if (spiaggiaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in spiaggiaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
                    (int, int) rigaEColonna = (ValueTuple<int, int>)item.Tag;
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
