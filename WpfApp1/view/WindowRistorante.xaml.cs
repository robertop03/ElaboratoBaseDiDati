using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per WindowRistorante.xaml
    /// </summary>
    public partial class WindowRistorante : Window
    {
        public WindowRistorante()
        {
            InitializeComponent();
        }

        private const double MinimumSpacing = 20;
        private const double EdgeSpacing = 20; // Spazio minimo dai bordi del Canvas
        private const double TableSize = 50;

        private double currentLeft = EdgeSpacing; // Posizione corrente sull'asse X
        private double currentTop = EdgeSpacing; // Posizione corrente sull'asse Y

        private void salaCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentLeft + TableSize + MinimumSpacing > salaCanvas.ActualWidth - EdgeSpacing ||
                currentTop + TableSize + MinimumSpacing > salaCanvas.ActualHeight - EdgeSpacing)
            {
                _ = MessageBox.Show("Non c'è abbastanza spazio per aggiungere un altro tavolo.", "Spazio esaurito", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Esci dal metodo
            }
            MessageBoxResult result = MessageBox.Show("Vuoi davvero aggiungere un tavolo?", "Conferma", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Verifica della risposta dell'utente
            if (result == MessageBoxResult.Yes)
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

                currentLeft += TableSize + MinimumSpacing;
                if (currentLeft + TableSize > salaCanvas.ActualWidth - EdgeSpacing)
                {
                    currentLeft = EdgeSpacing;
                    currentTop += TableSize + MinimumSpacing;
                }
            }
        }
    }
}
