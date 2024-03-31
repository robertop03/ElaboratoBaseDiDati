using System;

namespace WpfApp1.model.impl
{
    internal class Prenotazione
    {
        private static int id = 1;

        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public int RigaOmbrellonePrenotato { get; set; }
        public int ColonnaOmbrellonePrenotato { get; set; }
        public int Id { get; }
        public string CodiceFiscalePrenotante { get; set; }

        public Prenotazione(DateTime dataInizio, DateTime dataFine, int rigaOmbrellone, int colonnaOmbrellone, string codiceFiscalePrenotante)
        {
            Id = id++;
            DataInizio = dataInizio;
            DataFine = dataFine;
            RigaOmbrellonePrenotato = rigaOmbrellone;
            ColonnaOmbrellonePrenotato = colonnaOmbrellone;
            CodiceFiscalePrenotante = codiceFiscalePrenotante;
        }
    }
}
