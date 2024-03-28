using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Prenotazione(DateTime dataInizio, DateTime dataFine, int rigaOmbrellone, int colonnaOmbrellone)
        {
            Id = id++;
            DataInizio = dataInizio;
            DataFine = dataFine;
            RigaOmbrellonePrenotato = rigaOmbrellone;
            ColonnaOmbrellonePrenotato = colonnaOmbrellone;
        }
    }
}
