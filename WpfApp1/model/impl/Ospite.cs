using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.model.impl
{
    internal class Ospite : Persona
    {
        public string Nickname { get; set; }

        public Ospite(string codiceFiscale, string nome, string cognome, string numeroTelefono, string nickname) : base(codiceFiscale, nome, cognome, numeroTelefono)
        {
            Nickname = nickname;
        }
    }
}
