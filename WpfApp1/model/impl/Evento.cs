using System;
using System.Collections.Generic;

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

        public Evento(string titolo, DateTime data, TimeSpan orario, string descrizione, double costoIngresso, List<Ospite> ospiti)
        {
            Titolo = titolo;
            Data = data;
            Orario = orario;
            Descrizione = descrizione;
            Ospiti = new List<Ospite>(ospiti);
            CostoIngresso = costoIngresso;
        }

        public override string ToString()
        {
            string result = $"Evento: {Titolo}, Data: {Data}, Ora: {Orario}, Costo ingresso: {CostoIngresso}";
            result += $"\nDescrizione: {Descrizione}";
            result += "\nOspiti:";
            if (Ospiti.Count == 0)
            {
                result += $" nessuno";
            }
            foreach (Ospite ospite in Ospiti)
            {
                result += $"\n- {ospite}";
            }

            return result;
        }
    }
}
