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

        void RimuoviOmbrellone(Ombrellone ombrellone);

        void RimuoviTavolo(Tavolo tavolo);

        void PrenotaOmbrellone(Ombrellone ombrellone);

        void PrenotaTavolo(Tavolo tavolo);

        void DisdiciOmbrellone(Ombrellone ombrellone);

        void DisdiciTavolo(Tavolo tavolo);

        int GetNumeroTavoli();

        int GetNumeroOmbrelloni();

        string InfoTavolo();

        string InfoOmbrellone();
    }
}
