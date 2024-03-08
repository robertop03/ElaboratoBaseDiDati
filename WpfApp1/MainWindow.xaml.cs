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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.view;

namespace WpfApp1
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void btnSpiaggia_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            WindowSpiaggia windowSpiaggia = new WindowSpiaggia();
            windowSpiaggia.Closed += (s, args) => Instance.Show();
            windowSpiaggia.Show();
        }

        private void btnRistorante_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            WindowRistorante windowRistorante = new WindowRistorante();
            windowRistorante.Closed += (s, args) => Instance.Show();
            windowRistorante.Show();
        }

        private void btnBilanci_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            WindowBilanci windowBilanci = new WindowBilanci();
            windowBilanci.Closed += (s, args) => Instance.Show();
            windowBilanci.Show();
        }
    }
}
