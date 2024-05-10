using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.controller.impl;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per SetPricesDialog.xaml
    /// </summary>
    public partial class SetPricesDialog : Window
    {

        public bool Result { get; private set; }
        public double PrimaBassa { get; private set; }
        public double PrimaAlta { get; private set; }
        public double SecondaBassa { get; private set; }
        public double SecondaAlta { get; private set; }
        public double AltreBassa { get; private set; }
        public double AltreAlta { get; private set; }

        private readonly List<string> prezziFromDb = new List<string>();

        internal SetPricesDialog(ControllerImpl controller)
        {
            InitializeComponent();
            prezziFromDb = controller.GetPrezziOmbrelloni();

            if (prezziFromDb.Count > 0)
            {
                txtPrezzoPrimaFilaBassaStagione.Text = prezziFromDb[0];
                txtPrezzoPrimaFilaAltaStagione.Text = prezziFromDb[1];
                txtPrezzoSecondaFilaBassaStagione.Text = prezziFromDb[2];
                txtPrezzoSecondaFilaAltaStagione.Text = prezziFromDb[3];
                txtPrezzoAltreFileBassaStagione.Text = prezziFromDb[4];
                txtPrezzoAltreFileAltaStagione.Text = prezziFromDb[5];
            }

            _ = txtPrezzoPrimaFilaBassaStagione.Focus();
        }

        private void btnConfermaImpostazionePrezzi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<(TextBox, string)> fieldsToCheck = new List<(TextBox, string)>()
                {
                    (txtPrezzoPrimaFilaBassaStagione, "Prezzo prima fila bassa stagione"),
                    (txtPrezzoSecondaFilaBassaStagione, "Prezzo seconda fila bassa stagione"),
                    (txtPrezzoAltreFileBassaStagione, "Prezzo altre file bassa stagione"),
                    (txtPrezzoPrimaFilaAltaStagione, "Prezzo prima fila alta stagione"),
                    (txtPrezzoSecondaFilaAltaStagione, "Prezzo seconda fila alta stagione"),
                    (txtPrezzoAltreFileAltaStagione, "Prezzo altre file alta stagione")
                };

                foreach ((TextBox, string) field in fieldsToCheck)
                {
                    CheckField(field.Item1, field.Item2);
                }
                PrimaBassa = double.Parse(txtPrezzoPrimaFilaBassaStagione.Text);
                PrimaAlta = double.Parse(txtPrezzoPrimaFilaAltaStagione.Text);
                SecondaBassa = double.Parse(txtPrezzoSecondaFilaBassaStagione.Text);
                SecondaAlta = double.Parse(txtPrezzoSecondaFilaAltaStagione.Text);
                AltreBassa = double.Parse(txtPrezzoAltreFileBassaStagione.Text);
                AltreAlta = double.Parse(txtPrezzoAltreFileAltaStagione.Text);

                Result = true;
                Close();
            }
            catch (Exception ex)
            {
                Result = false;
                _ = MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckField(TextBox textBox, string fieldName)
        {
            string fieldValue = textBox.Text.Trim();

            if (string.IsNullOrEmpty(fieldValue))
            {
                _ = textBox.Focus();
                throw new ArgumentNullException($"{fieldName} non inserito.");
            }

            if (!IsStringAllNumericAndBetweenRange(fieldValue))
            {
                _ = textBox.Focus();
                textBox.SelectAll();
                throw new Exception($"I prezzi di {fieldName} devono contenere solo numeri compresi tra 5 e 100 (inclusi)");
            }
        }

        private bool IsStringAllNumericAndBetweenRange(string str)
        {
            return double.TryParse(str, out double numericValue) && numericValue >= 5 && numericValue <= 100;
        }
    }
}
