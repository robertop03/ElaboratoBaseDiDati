using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.model.impl
{
    internal abstract class Persona
    {
        public string CodiceFiscale { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string NumeroTelefono { get; set; }

        public Persona(string codiceFiscale, string cognome, string nome, string numeroTelefono)
        {
            CodiceFiscale = codiceFiscale;
            Nome = nome;
            Cognome = cognome;
            NumeroTelefono = numeroTelefono;
        }
    }
}
