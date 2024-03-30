using WpfApp1.model.api;

namespace WpfApp1.model.impl
{
    internal class Ombrellone : IOmbrellone
    {
        public int NumeroRiga { get; set; }
        public int NumeroColonna { get; set; }
        public double PrezzoGiornaliero { get; set; }

        public Ombrellone(int numeroRiga, int numeroColonna, double prezzoGiornaliero)
        {
            NumeroRiga = numeroRiga;
            NumeroColonna = numeroColonna;
            PrezzoGiornaliero = prezzoGiornaliero;
        }

        public string InfoOmbrellone()
        {
            string infoOmbrellone = string.Empty;
            infoOmbrellone += "ombrellone n°: " + NumeroRiga + "(riga) " + NumeroColonna + "(colonna)";
            return infoOmbrellone;
        }

    }
}
