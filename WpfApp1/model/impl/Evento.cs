using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.model.impl
{
    internal class Evento
    {
        public string Titolo { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Orario { get; set; }
        public string Descrizione { get; set; }
        public List<Ospite> Ospiti { get; set; }
        public double CostoIngresso { get; set; }

        // Costruttore
        public Evento(string titolo, DateTime data, TimeSpan orario, string descrizione, double costoIngresso)
        {
            Titolo = titolo;
            Data = data;
            Orario = orario;
            Descrizione = descrizione;
            Ospiti = new List<Ospite>();
            CostoIngresso = costoIngresso;
        }

        // Metodo per aggiungere un ospite all'evento
        public void AggiungiOspite(Ospite ospite)
        {
            Ospiti.Add(ospite);
        }
    }
}
