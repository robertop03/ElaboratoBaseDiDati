namespace WpfApp1.model.impl
{
    internal class ScontoOmbrellone
    {
        public int NumeroGiorni { get; set; }
        public double PercentualeSconto { get; set; }

        public ScontoOmbrellone(int numeroGiorni, double percentualeSconto)
        {
            NumeroGiorni = numeroGiorni;
            PercentualeSconto = percentualeSconto;
        }

        public override string ToString()
        {
            return $"Numero giorni: {NumeroGiorni} -> Sconto: {PercentualeSconto}%";
        }
    }
}
