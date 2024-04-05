using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.view
{
    /// <summary>
    /// Logica di interazione per SetPricesDialog.xaml
    /// </summary>
    public partial class SetPricesDialog : Window
    {
        public bool Result { get; set; }
        public SetPricesDialog()
        {
            InitializeComponent();
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
