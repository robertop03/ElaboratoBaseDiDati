namespace WpfApp1.model.api
{
    internal interface IOmbrellone
    {
        int NumeroRiga { get; set; }

        int NumeroColonna { get; set; }

        void Prenota();

        void Disdici();
    }
}
