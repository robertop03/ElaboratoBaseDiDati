using System;

namespace WpfApp1.model.impl
{
    internal enum Pasto
    {
        Pranzo,
        Cena
    }

    internal class PrenotazioneTavolo
    {
        public DateTime Data { get; set; }
        public Pasto Pasto { get; set; }
        public string CodiceFiscalePrenotante { get; set; }
        public int IdTavolo { get; set; }
        public int NumeroPersonePrenotanti { get; set; }

        public PrenotazioneTavolo(DateTime data, Pasto pasto, int idTavolo, string codiceFiscalePrenotante, int numeroPersonePrenotanti)
        {
            Data = data;
            Pasto = pasto;
            CodiceFiscalePrenotante = codiceFiscalePrenotante;
            IdTavolo = idTavolo;
            NumeroPersonePrenotanti = numeroPersonePrenotanti;
        }
    }
}
