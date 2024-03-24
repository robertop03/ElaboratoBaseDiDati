using WpfApp1.model.api;

namespace WpfApp1.model.impl
{
    internal class Ombrellone : AbstractOggettoPrenotabile, IOmbrellone
    {
        public int NumeroRiga { get; set; }
        public int NumeroColonna { get; set; }

        public Ombrellone(int numeroRiga, int numeroColonna)
        {
            NumeroRiga = numeroRiga;
            NumeroColonna = numeroColonna;
            Occupato = false;
        }

        public string InfoOmbrellone()
        {
            string infoOmbrellone = string.Empty;
            infoOmbrellone += "ombrellone n°: " + NumeroRiga + "(riga) " + NumeroColonna + "(colonna)";
            infoOmbrellone += Occupato ? "stato = occupato" : "non occupato";
            return infoOmbrellone;
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
