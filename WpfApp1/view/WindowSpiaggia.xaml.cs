using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per WindowSpiaggia.xaml
    /// </summary>
    public partial class WindowSpiaggia : Window
    {
        public WindowSpiaggia()
        {
            InitializeComponent();
        }

        private const double MinimumSpacing = 17; // Spazio minimo tra gli ombrelloni
        private const double EdgeSpacing = 20; // Spazio minimo dai bordi del Canvas
        private const double UmbrellaSize = 50;

        private double currentLeft = EdgeSpacing; // Posizione corrente sull'asse X
        private double currentTop = EdgeSpacing; // Posizione corrente sull'asse Y

        private void spiaggiaCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
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
                // Crea un nuovo ombrellone (ad esempio, un'ellisse gialla)

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

                // Aggiorna la posizione corrente per il prossimo ombrello
                currentLeft += UmbrellaSize + MinimumSpacing;
                if (currentLeft + UmbrellaSize > spiaggiaCanvas.ActualWidth - EdgeSpacing)
                {
                    currentLeft = EdgeSpacing;
                    currentTop += UmbrellaSize + MinimumSpacing;
                }
            }
        }
    }
}
