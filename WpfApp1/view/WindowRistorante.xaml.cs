using System;
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
        private int idTavolo = 1;


        public WindowRistorante()
        {
            InitializeComponent();
            controller = new ControllerImpl();
            DataContext = controller;
            controller.AggiuntoTavolo += Controller_ModifiedNumberTables;
            controller.RimossoTavolo += Controller_ModifiedNumberTables;
            lblNumeroTavoli.Content = "Numero tavoli: 0";

        }

        private const double MinimumSpacing = 20;
        private const double EdgeSpacing = 20; // Spazio minimo dai bordi del Canvas
        private const double TableSize = 50;

        private double currentLeft = EdgeSpacing; // Posizione corrente sull'asse X
        private double currentTop = EdgeSpacing; // Posizione corrente sull'asse Y

        private void Controller_ModifiedNumberTables(object sender, EventArgs e)
        {
            // Aggiorna il contenuto della Label ogni volta che viene aggiunto un ombrellone
            lblNumeroTavoli.Content = "Numero tavoli: " + controller.GetNumeroTavoli();
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
                            controller.DisdiciTavolo((int)item.Tag);
                            image.Source = new BitmapImage(new Uri("../resources/table_icon.png", UriKind.Relative));
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
                            controller.PrenotaTavolo((int)item.Tag);
                            image.Source = new BitmapImage(new Uri("../resources/table_icon_booked.png", UriKind.Relative));
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
    }
}
