using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApp1.controller.api;
using WpfApp1.model.impl;

namespace WpfApp1.controller.impl
{
    internal class ControllerImpl : IController
    {
        public ObservableCollection<Ombrellone> ListaOmbrelloni { get; set; }
        public ObservableCollection<Tavolo> ListaTavoli { get; set; }
        public ObservableCollection<Prenotazione> ListaPrenotazioniOmbrelloni { get; set; }

        public event EventHandler AggiuntoOmbrellone;
        public event EventHandler RimossoOmbrellone;
        public event EventHandler AggiuntoTavolo;
        public event EventHandler RimossoTavolo;

        public ControllerImpl()
        {
            ListaOmbrelloni = new ObservableCollection<Ombrellone>();
            ListaTavoli = new ObservableCollection<Tavolo>();
            ListaPrenotazioniOmbrelloni = new ObservableCollection<Prenotazione>();
        }

        public void AggiungiOmbrellone(int numeroRiga, int numeroColonna, double prezzoGiornaliero)
        {
            Ombrellone ombrellone = new Ombrellone(numeroRiga, numeroColonna, prezzoGiornaliero);
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
                    ListaPrenotazioniOmbrelloni.Add(prenotazione);
                }
            }
        }

        public bool ControlloOmbrelloneLibero(int numeroRiga, int numeroColonna, DateTime dataInizio, DateTime dataFine)
        {
            for (int i = ListaPrenotazioniOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaPrenotazioniOmbrelloni[i].RigaOmbrellonePrenotato == numeroRiga &&
                    ListaPrenotazioniOmbrelloni[i].ColonnaOmbrellonePrenotato == numeroColonna &&
                    ((dataInizio >= ListaPrenotazioniOmbrelloni[i].DataInizio && dataInizio <= ListaPrenotazioniOmbrelloni[i].DataFine) ||
                    (dataFine >= ListaPrenotazioniOmbrelloni[i].DataInizio && dataFine <= ListaPrenotazioniOmbrelloni[i].DataFine) ||
                    (dataInizio <= ListaPrenotazioniOmbrelloni[i].DataInizio && dataFine >= ListaPrenotazioniOmbrelloni[i].DataFine)))
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

        public List<(int, int)> OmbrelloniPrenotati(DateTime data)
        {
            List<(int, int)> ombrelloniPrenotati = new List<(int, int)>();

            foreach (Prenotazione prenotazione in ListaPrenotazioniOmbrelloni)
            {
                if (data.Date >= prenotazione.DataInizio.Date && data.Date <= prenotazione.DataFine.Date)
                {
                    ombrelloniPrenotati.Add((prenotazione.RigaOmbrellonePrenotato, prenotazione.ColonnaOmbrellonePrenotato));
                }
            }
            return ombrelloniPrenotati;
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

        public string InfoOmbrellone(int riga, int colonna)
        {
            string info = "";

            return info;
        }
    }
}
