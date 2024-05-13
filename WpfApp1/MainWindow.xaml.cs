using System.Windows;
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

        private void btnEventi_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            AddEvento_Ospite addEvento_Ospite = new AddEvento_Ospite();
            addEvento_Ospite.Closed += (s, args) => Instance.Show();
            addEvento_Ospite.Show();
        }
    }
}
