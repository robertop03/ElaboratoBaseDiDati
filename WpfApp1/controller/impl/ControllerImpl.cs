using System;
using System.Collections.ObjectModel;
using WpfApp1.controller.api;
using WpfApp1.model.impl;

namespace WpfApp1.controller.impl
{
    internal class ControllerImpl : IController
    {
        public ObservableCollection<Ombrellone> ListaOmbrelloni { get; set; }
        public ObservableCollection<Tavolo> ListaTavoli { get; set; }
        public ObservableCollection<Prenotazione> ListaPrenotazioni { get; set; }

        public event EventHandler AggiuntoOmbrellone;
        public event EventHandler RimossoOmbrellone;
        public event EventHandler AggiuntoTavolo;
        public event EventHandler RimossoTavolo;

        public ControllerImpl()
        {
            ListaOmbrelloni = new ObservableCollection<Ombrellone>();
            ListaTavoli = new ObservableCollection<Tavolo>();
            ListaPrenotazioni = new ObservableCollection<Prenotazione>();
        }

        public void AggiungiOmbrellone(int numeroRiga, int numeroColonna)
        {
            Ombrellone ombrellone = new Ombrellone(numeroRiga, numeroColonna);
            ListaOmbrelloni.Add(ombrellone);
            AggiuntoOmbrellone?.Invoke(this, EventArgs.Empty);
        }

        public void AggiungiTavolo(int idTavolo, int numeroPosti)
        {
            Tavolo tavolo = new Tavolo(idTavolo, numeroPosti);
            ListaTavoli.Add(tavolo);
            AggiuntoTavolo?.Invoke(this, EventArgs.Empty);
        }

        public void DisdiciOmbrellone(int numeroRiga, int numeroColonna)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    // ListaOmbrelloni[i].Disdici();
                }
            }
        }

        public void DisdiciTavolo(int idTavolo)
        {
            for (int i = ListaTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaTavoli[i].IdTavolo == idTavolo)
                {
                    // ListaTavoli[i].Disdici();
                }
            }
        }

        public void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    Prenotazione prenotazione = new Prenotazione(dataInzio, dataFine, numeroRiga, numeroColonna);
                    ListaPrenotazioni.Add(prenotazione);
                }
            }
        }

        public bool ControlloOmbrelloneLibero(int numeroRiga, int numeroColonna, DateTime dataInizio, DateTime dataFine)
        {
            for (int i = ListaPrenotazioni.Count - 1; i >= 0; i--)
            {
                if (ListaPrenotazioni[i].RigaOmbrellonePrenotato == numeroRiga &&
                    ListaPrenotazioni[i].ColonnaOmbrellonePrenotato == numeroColonna &&
                    ((dataInizio >= ListaPrenotazioni[i].DataInizio && dataInizio <= ListaPrenotazioni[i].DataFine) ||
                    (dataFine >= ListaPrenotazioni[i].DataInizio && dataFine <= ListaPrenotazioni[i].DataFine) ||
                    (dataInizio <= ListaPrenotazioni[i].DataInizio && dataFine >= ListaPrenotazioni[i].DataFine)))
                {
                    return false;
                }
            }
            return true;
        }

        public void PrenotaTavolo(int idTavolo)
        {
            for (int i = ListaTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaTavoli[i].IdTavolo == idTavolo)
                {
                    // ListaTavoli[i].Prenota();
                }
            }
        }

        public void RimuoviOmbrellone(int numeroRiga, int numeroColonna)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    ListaOmbrelloni.RemoveAt(i);
                }
            }
            RimossoOmbrellone?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviTavolo(int idTavolo)
        {
            for (int i = ListaTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaTavoli[i].IdTavolo == idTavolo)
                {
                    ListaTavoli.RemoveAt(i);
                }
            }
            RimossoTavolo?.Invoke(this, EventArgs.Empty);
        }

        public int GetNumeroTavoli()
        {
            return ListaTavoli.Count;
        }

        public int GetNumeroOmbrelloni()
        {
            return ListaOmbrelloni.Count;
        }

        public string InfoTavolo()
        {
            throw new NotImplementedException();
        }

        public string InfoOmbrellone()
        {
            throw new NotImplementedException();
        }
    }
}
