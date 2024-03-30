using WpfApp1.model.api;

namespace WpfApp1.model.impl
{
    internal class Tavolo : ITavolo
    {
        public Tavolo(int idTavolo, int numeroPosti)
        {
            IdTavolo = idTavolo;
            NumeroPosti = numeroPosti;
        }

        public int NumeroPosti { get; set; }
        public int IdTavolo { get; set; }

        public string InfoTavolo()
        {
            string infoTavolo = string.Empty;
            infoTavolo += "id tavolo: " + IdTavolo;
            infoTavolo += "numero posti: " + NumeroPosti;
            return infoTavolo;
        }
    }
}
