namespace WpfApp1.model.api
{
    internal interface ITavolo
    {
        int NumeroPosti { get; set; }

        string InfoTavolo();

        void Prenota();

        void Disdici();
    }
}
