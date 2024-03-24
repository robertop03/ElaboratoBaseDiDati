using WpfApp1.model.api;

namespace WpfApp1.model.impl
{
    internal class Tavolo : AbstractOggettoPrenotabile, ITavolo
    {
        public Tavolo(int idTavolo, int numeroPosti)
        {
            IdTavolo = idTavolo;
            NumeroPosti = numeroPosti;
            Occupato = false;
        }

        public int NumeroPosti { get; set; }
        public int IdTavolo { get; set; }

        public string InfoTavolo()
        {
            string infoTavolo = string.Empty;
            infoTavolo += "id tavolo: " + IdTavolo;
            infoTavolo += Occupato ? "stato = occupato" : "non occupato";
            infoTavolo += "numero posti: " + NumeroPosti;
            return infoTavolo;
        }

        public void Prenota()
        {
            Occupato = true;
        }

        public void Disdici()
        {
            Occupato = false;
        }
    }
}
