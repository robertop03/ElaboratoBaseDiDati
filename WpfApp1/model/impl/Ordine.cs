using System.Collections.Generic;

namespace WpfApp1.model.impl
{
    internal class Ordine
    {
        // ogni ordine contiene il suo id, la prenotazione alla quale fa riferimento, un elenco di menù ordinati e elenco di piatti ordinati
        public int IdOrdine { get; set; }
        public PrenotazioneTavolo RiferimentoPrenotazione { get; set; }
        public List<Piatto> PiattiOrdinati { get; set; }
        public List<Menu> MenuOrdinati { get; set; }

        public Ordine(int idOrdine, PrenotazioneTavolo prenotazioneTavolo, List<Piatto> piattiOrdinati, List<Menu> menuOrdinati)
        {
            IdOrdine = idOrdine;
            RiferimentoPrenotazione = prenotazioneTavolo;
            PiattiOrdinati = piattiOrdinati;
            MenuOrdinati = menuOrdinati;
        }
    }
}
