using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PrenotazioneTavolo(DateTime data, Pasto pasto, string codiceFiscalePrenotante)
        {
            Data = data;
            Pasto = pasto;
            CodiceFiscalePrenotante = codiceFiscalePrenotante;
        }
    }
}
