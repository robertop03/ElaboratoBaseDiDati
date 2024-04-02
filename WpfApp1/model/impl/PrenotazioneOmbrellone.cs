using System;

namespace WpfApp1.model.impl
{
    internal class PrenotazioneOmbrellone
    {

        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public int RigaOmbrellonePrenotato { get; set; }
        public int ColonnaOmbrellonePrenotato { get; set; }
        public string CodiceFiscalePrenotante { get; set; }

        public PrenotazioneOmbrellone(DateTime dataInizio, DateTime dataFine, int rigaOmbrellone, int colonnaOmbrellone, string codiceFiscalePrenotante)
        {
            DataInizio = dataInizio;
            DataFine = dataFine;
            RigaOmbrellonePrenotato = rigaOmbrellone;
            ColonnaOmbrellonePrenotato = colonnaOmbrellone;
            CodiceFiscalePrenotante = codiceFiscalePrenotante;
        }
    }
}
