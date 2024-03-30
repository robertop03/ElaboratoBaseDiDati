using System;

namespace WpfApp1.controller.api
{
    internal interface IController
    {
        void AggiungiOmbrellone(int numeroRiga, int numeroColonna, double prezzoGiornaliero);

        void AggiungiTavolo(int idTavolo, int numeroPosti);

        void RimuoviOmbrellone(int numeroRiga, int numeroColonna);

        void RimuoviTavolo(int idTavolo);

        void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine);

        bool ControlloOmbrelloneLibero(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine);

        // bool ControlloTavoloLibero();

        void PrenotaTavolo(int idTavolo);

        void DisdiciOmbrellone(int numeroRiga, int numeroColonna);

        void DisdiciTavolo(int idTavolo);

        int GetNumeroTavoli();

        int GetNumeroOmbrelloni();

        string InfoTavolo();

        string InfoOmbrellone(int riga, int colonna);

    }
}
