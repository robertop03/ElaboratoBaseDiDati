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
        public ObservableCollection<PrenotazioneOmbrellone> ListaPrenotazioniOmbrelloni { get; set; }
        public ObservableCollection<PrenotazioneTavolo> ListaPrenotazioniTavoli { get; set; }
        public ObservableCollection<Cliente> ListaClienti { get; set; }

        public event EventHandler AggiuntoOmbrellone;
        public event EventHandler RimossoOmbrellone;
        public event EventHandler AggiuntoTavolo;
        public event EventHandler RimossoTavolo;

        public ControllerImpl()
        {
            ListaOmbrelloni = new ObservableCollection<Ombrellone>();
            ListaTavoli = new ObservableCollection<Tavolo>();
            ListaPrenotazioniOmbrelloni = new ObservableCollection<PrenotazioneOmbrellone>();
            ListaClienti = new ObservableCollection<Cliente>();
            ListaPrenotazioniTavoli = new ObservableCollection<PrenotazioneTavolo>();
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

        public void DisdiciOmbrellone(DateTime dataInizio, DateTime dataFine, int numeroRiga, int numeroColonna)
        {
            foreach (PrenotazioneOmbrellone prenotazione in ListaPrenotazioniOmbrelloni)
            {
                if (prenotazione.DataInizio == dataInizio &&
                    prenotazione.DataFine == dataFine &&
                    prenotazione.RigaOmbrellonePrenotato == numeroRiga &&
                    prenotazione.ColonnaOmbrellonePrenotato == numeroColonna)
                {
                    _ = ListaPrenotazioniOmbrelloni.Remove(prenotazione);
                    break;
                }
            }
        }

        public double GetPrezzoGiornalieroOmbrellone(int numeroRiga, int numeroColonna)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    return ListaOmbrelloni[i].PrezzoGiornaliero;
                }
            }
            return double.NaN;
        }

        public void DisdiciTavolo(int idTavolo, DateTime data, string pasto)
        {
            foreach (PrenotazioneTavolo prenotazione in ListaPrenotazioniTavoli)
            {
                if (prenotazione.Data == data &&
                    prenotazione.IdTavolo == idTavolo &&
                    prenotazione.Pasto.ToString() == pasto)
                {
                    _ = ListaPrenotazioniTavoli.Remove(prenotazione);
                    break;
                }
            }
        }

        public void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine, string codiceFiscalePrenotante)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    PrenotazioneOmbrellone prenotazione = new PrenotazioneOmbrellone(dataInzio, dataFine, numeroRiga, numeroColonna, codiceFiscalePrenotante);
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

        public bool ControlloTavoloLibero(int idTavolo, DateTime data, string pasto)
        {
            for (int i = ListaPrenotazioniTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaPrenotazioniTavoli[i].IdTavolo == idTavolo && ListaPrenotazioniTavoli[i].Data == data && ListaPrenotazioniTavoli[i].Pasto.ToString() == pasto)
                {
                    return false;
                }
            }
            return true;
        }

        public void PrenotaTavolo(int idTavolo, DateTime data, string pasto, string codiceFiscalePrenotante)
        {
            for (int i = ListaTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaTavoli[i].IdTavolo == idTavolo)
                {
                    Pasto pastoEnum = (Pasto)Enum.Parse(typeof(Pasto), pasto);
                    PrenotazioneTavolo prenotazione = new PrenotazioneTavolo(data, pastoEnum, idTavolo, codiceFiscalePrenotante);
                    ListaPrenotazioniTavoli.Add(prenotazione);
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

            foreach (PrenotazioneOmbrellone prenotazione in ListaPrenotazioniOmbrelloni)
            {
                if (data.Date >= prenotazione.DataInizio.Date && data.Date <= prenotazione.DataFine.Date)
                {
                    ombrelloniPrenotati.Add((prenotazione.RigaOmbrellonePrenotato, prenotazione.ColonnaOmbrellonePrenotato));
                }
            }
            return ombrelloniPrenotati;
        }

        public void AggiungiCliente(string nome, string cognome, string numeroTelefono, int numeroPersoneOspiti, string città, string via, int civico, string email, string codiceDocumento, string codiceFiscale)
        {
            Cliente cliente = new Cliente(numeroPersoneOspiti, città, via, civico, email, codiceDocumento, codiceFiscale, nome, cognome, numeroTelefono);
            ListaClienti.Add(cliente);
        }

        public List<string> GetPrenotazioniOmbrellone(int numeroRiga, int numeroColonna)
        {
            List<string> toReturn = new List<string>();
            foreach (PrenotazioneOmbrellone prenotazione in ListaPrenotazioniOmbrelloni)
            {
                if (prenotazione.RigaOmbrellonePrenotato == numeroRiga &&
                    prenotazione.ColonnaOmbrellonePrenotato == numeroColonna)
                {
                    toReturn.Add("Pren. da " + prenotazione.DataInizio.ToString("dd/MM/yyyy") + " a " + prenotazione.DataFine.ToString("dd/MM/yyyy") + " da " + prenotazione.CodiceFiscalePrenotante);
                }
            }
            return toReturn;
        }

        public List<string> GetPrenotazioniTavolo(int idTavolo)
        {
            List<string> toReturn = new List<string>();
            foreach (PrenotazioneTavolo prenotazione in ListaPrenotazioniTavoli)
            {
                if (prenotazione.IdTavolo == idTavolo)
                {
                    toReturn.Add("Pren. il " + prenotazione.Data.ToString("dd/MM/yyyy") + " a " + prenotazione.Pasto.ToString() + " da " + prenotazione.CodiceFiscalePrenotante);
                }
            }
            return toReturn;
        }

        public bool NumeroPostiTavoloAdegueato(int idTavolo, int numeroOspiti)
        {
            for (int i = ListaTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaTavoli[i].IdTavolo == idTavolo)
                {
                    return ListaTavoli[i].NumeroPosti >= numeroOspiti;
                }
            }
            return false;
        }

        public int GetNumeroOmbrelloni()
        {
            return ListaOmbrelloni.Count;
        }

        public int GetNumeroTavoli()
        {
            return ListaTavoli.Count;
        }
    }
}
