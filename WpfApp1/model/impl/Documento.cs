using System;

namespace WpfApp1.model.impl
{
    public enum TipoDocumento
    {
        Patente,
        CartaIdentità,
        Passaporto
    }

    internal class Documento
    {
        private string codiceDocumento;

        public TipoDocumento Tipo { get; set; }
        public string CodiceDocumento
        {
            get => codiceDocumento;
            set => codiceDocumento = value.Length >= 1 && value.Length <= 1000
                    ? value
                    : throw new ArgumentOutOfRangeException("Il numero del documento deve avere una lunghezza compresa tra tra 5 e 20.");
        }
        public string CodiceFiscaleCliente { get; set; }

        public Documento(string codiceDocumento, string codiceFiscaleCliente, TipoDocumento tipo)
        {
            Tipo = tipo;
            CodiceDocumento = codiceDocumento;
            CodiceFiscaleCliente = codiceFiscaleCliente;
        }

        public override string ToString()
        {
            return $"Tipo: {Tipo}, Codice documento: {CodiceDocumento}, Cf cliente: {CodiceFiscaleCliente}";
        }
    }
}
