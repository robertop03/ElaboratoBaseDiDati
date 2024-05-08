using System.Windows;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per PrezziOmbrelloniLettini.xaml
    /// </summary>
    public partial class PrezziOmbrelloniLettini : Window
    {
        private readonly ControllerImpl controller;

        internal PrezziOmbrelloniLettini(ControllerImpl controller)
        {
            InitializeComponent();
            this.controller = controller;
        }

        private void btnOmbrelloni_Click(object sender, RoutedEventArgs e)
        {
            Close();
            SetPricesDialog setPricesDialog = new SetPricesDialog(controller);
            _ = setPricesDialog.ShowDialog();
            int i = 1;
            if (setPricesDialog.Result)
            {
                controller.AggiungiPrezziOmbrelloni(i, "BassaStagione", setPricesDialog.PrimaBassa);
                controller.AggiungiPrezziOmbrelloni(i, "AltaStagione", setPricesDialog.PrimaAlta);
                i++;
                controller.AggiungiPrezziOmbrelloni(i, "BassaStagione", setPricesDialog.SecondaBassa);
                controller.AggiungiPrezziOmbrelloni(i, "AltaStagione", setPricesDialog.SecondaAlta);

                for (i = 3; i <= controller.GetNumeroRighe(); i++)
                {
                    controller.AggiungiPrezziOmbrelloni(i, "BassaStagione", setPricesDialog.AltreBassa);
                    controller.AggiungiPrezziOmbrelloni(i, "AltaStagione", setPricesDialog.AltreAlta);
                }
            }
        }

        private void btnLettini_Click(object sender, RoutedEventArgs e)
        {
            Close();
            SetPricesLettiniDialog setPricesLettiniDialog = new SetPricesLettiniDialog(controller);
            _ = setPricesLettiniDialog.ShowDialog();
            int i = 1;
            if (setPricesLettiniDialog.Result)
            {
                controller.AggiungiPrezziLettini(i, "BassaStagione", setPricesLettiniDialog.PrimaBassa);
                controller.AggiungiPrezziLettini(i, "AltaStagione", setPricesLettiniDialog.PrimaAlta);
                i++;
                controller.AggiungiPrezziLettini(i, "BassaStagione", setPricesLettiniDialog.SecondaBassa);
                controller.AggiungiPrezziLettini(i, "AltaStagione", setPricesLettiniDialog.SecondaAlta);

                for (i = 3; i <= controller.GetNumeroRighe(); i++)
                {
                    controller.AggiungiPrezziLettini(i, "BassaStagione", setPricesLettiniDialog.AltreBassa);
                    controller.AggiungiPrezziLettini(i, "AltaStagione", setPricesLettiniDialog.AltreAlta);
                }
            }
        }
    }
}
