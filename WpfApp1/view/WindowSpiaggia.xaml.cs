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
            controller.AggiuntoOmbrellone += Controller_AggiuntoOmbrellone;
            lblNumeroOmbrelloni.Content = "Numero ombrelloni: 0";
        }

        private const double MinimumSpacing = 17; // Spazio minimo tra gli ombrelloni
        private const double EdgeSpacing = 20; // Spazio minimo dai bordi del Canvas
        private const double UmbrellaSize = 50;

        private double currentLeft = EdgeSpacing; // Posizione corrente sull'asse X
        private double currentTop = EdgeSpacing; // Posizione corrente sull'asse Y

        private void Controller_AggiuntoOmbrellone(object sender, EventArgs e)
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

                // Aggiungi l'ombrello al Canvas
                _ = spiaggiaCanvas.Children.Add(newCheckBox);

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

        private void btnRimuoviOmbrellone_Click(object sender, RoutedEventArgs e)
        {
            if (spiaggiaCanvas.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true))
            {
                foreach (CheckBox item in spiaggiaCanvas.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true).ToList())
                {
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
