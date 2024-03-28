using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.model.impl;

namespace WpfApp1.controller.api
{
    internal interface IController
    {
        void AggiungiOmbrellone(int numeroRiga, int numeroColonna);

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

        string InfoOmbrellone();

    }
}
