using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.model.impl
{
    internal class Piatto
    {
        public string Nome { get; set; }
        public double Prezzo { get; set; }
        public string Descrizione { get; set; }

        public Piatto(string nome, double prezzo, string descrizione)
        {
            Nome = nome;
            Prezzo = prezzo;
            Descrizione = descrizione;
        }

        public override string ToString()
        {
            return $"Piatto: {Nome}, Prezzo: {Prezzo} €, Descrizione: {Descrizione}";
        }
    }
}
