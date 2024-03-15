using System.Collections.Generic;
using System.Windows;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per SelectSeatsDialog.xaml
    /// </summary>
    public partial class SelectSeatsDialog : Window
    {
        public int SelectedSeats { get; private set; }

        public List<int> Posti { get; private set; }

        public SelectSeatsDialog()
        {
            InitializeComponent();
            Posti = new List<int> { 2, 4, 6, 8, 10 }; // Opzioni per il numero di posti a sedere
            cmbNumeroPosti.ItemsSource = Posti;
            cmbNumeroPosti.SelectedIndex = 1; // Seleziona la prima opzione per impostazione predefinita
        }

        private void btnOkNumeroPosti_Click(object sender, RoutedEventArgs e)
        {
            SelectedSeats = (int)cmbNumeroPosti.SelectedItem;
            DialogResult = true; // Imposta il risultato della finestra di dialogo su "true" (OK)
        }
    }
}
