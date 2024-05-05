using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using WpfApp1.controller.api;
using WpfApp1.model.DB;
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
        public ObservableCollection<Menu> ListaMenu { get; set; }
        public ObservableCollection<Piatto> ListaPiatti { get; set; }
        public ObservableCollection<Ordine> ListaOrdini { get; set; }
        public ObservableCollection<Evento> ListaEventi { get; set; }
        public ObservableCollection<Ospite> ListaOspiti { get; set; }
        public ObservableCollection<ScontoOmbrellone> ListaScontiOmbrellone { get; set; }

        public event EventHandler AggiuntoOmbrellone;
        public event EventHandler RimossoOmbrellone;
        public event EventHandler AggiuntoTavolo;
        public event EventHandler RimossoTavolo;
        public event EventHandler AggiuntoPiatto;
        public event EventHandler RimossoPiatto;
        public event EventHandler AggiuntoMenu;
        public event EventHandler RimossoMenu;
        public event EventHandler AggiuntoEvento;
        public event EventHandler RimossoEvento;
        public event EventHandler AggiuntoOspite;
        public event EventHandler RimossoOspite;
        public event EventHandler AggiuntoSconto;
        public event EventHandler RimossoSconto;

        public static int IdMenu = 1;
        public static int IdOrdine = 1;

        public ControllerImpl()
        {
            ListaOmbrelloni = new ObservableCollection<Ombrellone>();
            ListaTavoli = new ObservableCollection<Tavolo>();
            ListaPrenotazioniOmbrelloni = new ObservableCollection<PrenotazioneOmbrellone>();
            ListaClienti = new ObservableCollection<Cliente>();
            ListaPrenotazioniTavoli = new ObservableCollection<PrenotazioneTavolo>();
            ListaMenu = new ObservableCollection<Menu>();
            ListaPiatti = new ObservableCollection<Piatto>();
            ListaOrdini = new ObservableCollection<Ordine>();
            ListaEventi = new ObservableCollection<Evento>();
            ListaOspiti = new ObservableCollection<Ospite>();
            ListaScontiOmbrellone = new ObservableCollection<ScontoOmbrellone>();
        }

        #region Ombrelloni

        public void AggiungiOmbrellone(int numeroRiga, int numeroColonna)
        {
            Ombrellone ombrellone = new Ombrellone(numeroRiga, numeroColonna);
            ListaOmbrelloni.Add(ombrellone);
            string query = $"INSERT INTO Ombrellone (Numero_riga, Numero_colonna) VALUES ({numeroRiga}, {numeroColonna})";
            DBConnect dbConnect = new DBConnect();
            dbConnect.Insert(query);
            AggiuntoOmbrellone?.Invoke(this, EventArgs.Empty);
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

        public void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine, string codiceFiscalePrenotante, int numeroLettiniAggiuntivi)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    PrenotazioneOmbrellone prenotazione = new PrenotazioneOmbrellone(dataInzio, dataFine, numeroRiga, numeroColonna, codiceFiscalePrenotante, numeroLettiniAggiuntivi);
                    ListaPrenotazioniOmbrelloni.Add(prenotazione);
                }
            }
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

        public int GetNumeroOmbrelloni()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT COUNT(DISTINCT CONCAT(numero_riga, '-', numero_colonna)) AS numero_ombrelloni FROM ombrellone;";
            DataTable dataTable = dbConnect.Select(query);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int nOmbrelloni = int.Parse(row["numero_ombrelloni"].ToString());
                return nOmbrelloni;
            }
            return 0;
        }

        public List<string> GetPrenotazioniOmbrellone(int numeroRiga, int numeroColonna)
        {
            List<string> toReturn = new List<string>();
            foreach (PrenotazioneOmbrellone prenotazione in ListaPrenotazioniOmbrelloni)
            {
                if (prenotazione.RigaOmbrellonePrenotato == numeroRiga &&
                    prenotazione.ColonnaOmbrellonePrenotato == numeroColonna)
                {
                    toReturn.Add("Pren. da " + prenotazione.DataInizio.ToString("dd/MM/yyyy") + " a " + prenotazione.DataFine.ToString("dd/MM/yyyy")
                        + " da " + prenotazione.CodiceFiscalePrenotante + ", lettini aggiunti: " + prenotazione.NumeroLettiniAggiuntivi);
                }
            }
            return toReturn;
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

        public void RimuoviTuttePrenotazioniOmbrellone(int numeroRiga, int numeroColonna)
        {
            List<PrenotazioneOmbrellone> prenotazioniDaRimuovere = ListaPrenotazioniOmbrelloni.Where(prenotazione => prenotazione.RigaOmbrellonePrenotato == numeroRiga && prenotazione.ColonnaOmbrellonePrenotato == numeroColonna).ToList();
            foreach (PrenotazioneOmbrellone prenotazione in prenotazioniDaRimuovere)
            {
                _ = ListaPrenotazioniOmbrelloni.Remove(prenotazione);
            }
        }

        public void LoadOmbrelloniFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM ombrellone;";
            DataTable dataTable = dbConnect.Select(query);
            foreach (DataRow row in dataTable.Rows)
            {
                int nRiga = int.Parse(row["Numero_riga"].ToString());
                int nColonna = int.Parse(row["Numero_colonna"].ToString());
                ListaOmbrelloni.Add(new Ombrellone(nRiga, nColonna));
            }
        }

        public List<(int, int)> GetOmbrelloni()
        {
            List<(int, int)> toReturn = new List<(int, int)>();
            foreach (Ombrellone ombrellone in ListaOmbrelloni)
            {
                toReturn.Add((ombrellone.NumeroRiga, ombrellone.NumeroColonna));
            }
            return toReturn;
        }

        public (int, int) RigaEColonna()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT numero_riga, numero_colonna FROM Ombrellone ORDER BY numero_riga DESC, numero_colonna DESC LIMIT 1;";
            DataTable dataTable = dbConnect.Select(query);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int nRiga = int.Parse(row["Numero_riga"].ToString());
                int nColonna = int.Parse(row["Numero_colonna"].ToString());
                return (nRiga, nColonna);
            }
            return (0, 0);
        }

        #endregion

        #region Tavoli

        public void AggiungiTavolo(int idTavolo, int numeroPosti)
        {
            Tavolo tavolo = new Tavolo(idTavolo, numeroPosti);
            ListaTavoli.Add(tavolo);
            string query = $"INSERT INTO Tavolo (Id_tavolo, Numero_posti) VALUES ({idTavolo}, {numeroPosti})";
            DBConnect dbConnect = new DBConnect();
            dbConnect.Insert(query);
            AggiuntoTavolo?.Invoke(this, EventArgs.Empty);
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

        public void PrenotaTavolo(int idTavolo, DateTime data, string pasto, string codiceFiscalePrenotante, int numeroPersonePrenotanti)
        {
            for (int i = ListaTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaTavoli[i].IdTavolo == idTavolo)
                {
                    Pasto pastoEnum = (Pasto)Enum.Parse(typeof(Pasto), pasto);
                    PrenotazioneTavolo prenotazione = new PrenotazioneTavolo(data, pastoEnum, idTavolo, codiceFiscalePrenotante, numeroPersonePrenotanti);
                    ListaPrenotazioniTavoli.Add(prenotazione);
                }
            }
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

        public int GetNumeroTavoli()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT COUNT(Id_tavolo) AS numero_tavoli FROM tavolo;";
            DataTable dataTable = dbConnect.Select(query);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int nTavoli = int.Parse(row["numero_tavoli"].ToString());
                return nTavoli;
            }
            return 0;
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

        public List<int> TavoliPrenotati(DateTime data)
        {
            List<int> tavoliPrenotati = new List<int>();

            foreach (PrenotazioneTavolo prenotazione in ListaPrenotazioniTavoli)
            {
                if (data.Date == prenotazione.Data.Date)
                {
                    tavoliPrenotati.Add(prenotazione.IdTavolo);
                }
            }
            return tavoliPrenotati;
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

        public void RimuoviTuttePrenotazioniTavolo(int idTavolo)
        {
            List<PrenotazioneTavolo> prenotazioniDaRimuovere = ListaPrenotazioniTavoli.Where(prenotazione => prenotazione.IdTavolo == idTavolo).ToList();
            foreach (PrenotazioneTavolo prenotazione in prenotazioniDaRimuovere)
            {
                _ = ListaPrenotazioniTavoli.Remove(prenotazione);
            }
        }

        public List<int> GetTavoli()
        {
            List<int> toReturn = new List<int>();
            foreach (Tavolo tavolo in ListaTavoli)
            {
                toReturn.Add(tavolo.IdTavolo);
            }
            return toReturn;
        }

        public int GetNumeroPostiTavolo(int idTavolo)
        {
            Tavolo tavolo = ListaTavoli.FirstOrDefault(t => t.IdTavolo == idTavolo);
            return tavolo != null ? tavolo.NumeroPosti : 0;
        }

        public void LoadTavoliFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM tavolo;";
            DataTable dataTable = dbConnect.Select(query);
            foreach (DataRow row in dataTable.Rows)
            {
                int idTavolo = int.Parse(row["Id_tavolo"].ToString());
                int nPosti = int.Parse(row["Numero_posti"].ToString());
                ListaTavoli.Add(new Tavolo(idTavolo, nPosti));
            }
        }

        public int GetLastIdTavolo()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT MAX(Id_tavolo) AS MaxId FROM tavolo;";
            DataTable dataTable = dbConnect.Select(query);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int idMax = int.Parse(row["MaxId"].ToString());
                return idMax;
            }
            return 0;
        }

        #endregion

        #region Menu, piatti e ordini

        public void AggiungiPiatto(string nome, double prezzo, string descrizione)
        {
            Piatto piatto = new Piatto(nome, prezzo, descrizione);
            ListaPiatti.Add(piatto);
            AggiuntoPiatto?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviPiatto(string nome)
        {
            for (int i = ListaPiatti.Count - 1; i >= 0; i--)
            {
                if (ListaPiatti[i].Nome == nome)
                {
                    ListaPiatti.RemoveAt(i);
                }
            }
            RimossoPiatto?.Invoke(this, EventArgs.Empty);
        }

        public void AggiungiMenu(int idMenu, double prezzo, List<string> piatti)
        {
            List<Piatto> listaPiatti = new List<Piatto>();
            Regex regex = new Regex(@"Piatto: (.+?), Prezzo: ([0-9.]+) €, Descrizione: (.+)");
            double totalePrezzoPiatti = 0;
            for (int i = piatti.Count - 1; i >= 0; i--)
            {
                Match match = regex.Match(piatti[i].ToString());
                string nome = match.Groups[1].Value;
                double prezzoPiatto = double.Parse(match.Groups[2].Value);
                Piatto piattoCorrispondente = ListaPiatti.FirstOrDefault(p => p.Nome == nome);
                if (piattoCorrispondente != null)
                {
                    totalePrezzoPiatti += prezzoPiatto;
                    listaPiatti.Add(piattoCorrispondente);
                }
            }
            if (prezzo < totalePrezzoPiatti)
            {
                Menu menu = new Menu(idMenu, listaPiatti, prezzo);
                ListaMenu.Add(menu);
                AggiuntoMenu?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                throw new Exception("Il prezzo del menù deve essere inferiore del totale del prezzo dei piatti che contiene.");
            }
        }

        public void RimuoviMenu(int idMenu)
        {
            for (int i = ListaMenu.Count - 1; i >= 0; i--)
            {
                if (ListaMenu[i].IdMenu == idMenu)
                {
                    ListaMenu.RemoveAt(i);
                }
            }
            RimossoMenu?.Invoke(this, EventArgs.Empty);
        }

        public List<string> GetPiatti()
        {
            List<string> toReturn = new List<string>();
            foreach (Piatto piatto in ListaPiatti)
            {
                toReturn.Add(piatto.ToString());
            }
            return toReturn;
        }

        public List<string> GetMenu()
        {
            List<string> toReturn = new List<string>();
            foreach (Menu menu in ListaMenu)
            {
                toReturn.Add(menu.ToString());
            }
            return toReturn;
        }

        public void AggiungiOrdine(int idOrdine, DateTime data, string pasto, int idTavolo, List<int> idMenuOrdinati, List<string> nomiPiattiOrdinati)
        {
            List<Piatto> piatti = new List<Piatto>();
            List<Menu> menu = new List<Menu>();
            int indexPrenotazioneTavolo = 0;
            for (int i = idMenuOrdinati.Count - 1; i >= 0; i--)
            {
                for (int j = ListaMenu.Count - 1; j >= 0; j--)
                {
                    if (idMenuOrdinati[i] == ListaMenu[j].IdMenu)
                    {
                        menu.Add(ListaMenu[j]);
                    }
                }
            }

            for (int i = nomiPiattiOrdinati.Count - 1; i >= 0; i--)
            {
                for (int j = ListaPiatti.Count - 1; j >= 0; j--)
                {
                    if (nomiPiattiOrdinati[i] == ListaPiatti[j].Nome)
                    {
                        piatti.Add(ListaPiatti[j]);
                    }
                }
            }
            for (int i = ListaPrenotazioniTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaPrenotazioniTavoli[i].Data == data && ListaPrenotazioniTavoli[i].IdTavolo == idTavolo && ListaPrenotazioniTavoli[i].Pasto.ToString() == pasto)
                {
                    indexPrenotazioneTavolo = i;
                }
            }
            Ordine ordine = new Ordine(idOrdine, ListaPrenotazioniTavoli[indexPrenotazioneTavolo], piatti, menu);
            ListaOrdini.Add(ordine);
        }
        #endregion

        #region Cliente
        public void AggiungiCliente(string nome, string cognome, string numeroTelefono, string città, string via, int civico, string email, string codiceDocumento, string codiceFiscale)
        {
            Cliente cliente = new Cliente(città, via, civico, email, codiceDocumento, codiceFiscale, nome, cognome, numeroTelefono);
            ListaClienti.Add(cliente);
        }

        public List<string> GetClienti()
        {
            List<string> toReturn = new List<string>();
            foreach (Cliente cliente in ListaClienti)
            {
                toReturn.Add(cliente.ToString());
            }
            return toReturn;
        }
        #endregion

        #region Eventi e Ospiti

        public void AggiungiEvento(string titolo, DateTime data, TimeSpan orario, string descrizione, double costoIngrezzo, List<string> ospitiStr)
        {
            List<Ospite> ospiti = new List<Ospite>();
            foreach (string ospiteStr in ospitiStr)
            {
                Regex regex = new Regex(@"CF: (?<cf>[^,]+)");
                Match match = regex.Match(ospiteStr);

                if (match.Success)
                {
                    string codiceFiscale = match.Groups["cf"].Value.Trim();
                    Ospite ospiteCorrente = ListaOspiti.FirstOrDefault(ospite => ospite.CodiceFiscale.Equals(codiceFiscale));

                    if (ospiteCorrente != null)
                    {
                        ospiti.Add(ospiteCorrente);
                    }
                }
            }

            Evento evento = new Evento(titolo, data, orario, descrizione, costoIngrezzo, ospiti);
            ListaEventi.Add(evento);
            AggiuntoEvento?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviEvento(string titolo, DateTime data)
        {
            for (int i = ListaEventi.Count - 1; i >= 0; i--)
            {
                if (ListaEventi[i].Titolo == titolo && ListaEventi[i].Data == data)
                {
                    ListaEventi.RemoveAt(i);
                }
            }
            RimossoEvento?.Invoke(this, EventArgs.Empty);
        }

        public List<string> GetEvents()
        {
            List<string> toReturn = new List<string>();
            foreach (Evento evento in ListaEventi)
            {
                toReturn.Add(evento.ToString());
            }
            return toReturn;
        }

        public void AggiungiOspite(string codiceFiscale, string cognome, string nome, string numeroTelefono, string nickname)
        {
            Ospite ospite = new Ospite(codiceFiscale, cognome, nome, numeroTelefono, nickname);
            ListaOspiti.Add(ospite);
            AggiuntoOspite?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviOspite(string codiceFiscale)
        {
            for (int i = ListaOspiti.Count - 1; i >= 0; i--)
            {
                if (ListaOspiti[i].CodiceFiscale == codiceFiscale)
                {
                    ListaOspiti.RemoveAt(i);
                }
            }
            RimossoOspite?.Invoke(this, EventArgs.Empty);
        }

        public List<string> GetOspiti()
        {
            List<string> toReturn = new List<string>();
            foreach (Ospite ospiti in ListaOspiti)
            {
                toReturn.Add(ospiti.ToString());
            }
            return toReturn;
        }

        #endregion

        #region Sconti

        public void AggiungiSconto(int numeroGiorni, double percentualeSconto)
        {
            ScontoOmbrellone scontoOmbrellone = new ScontoOmbrellone(numeroGiorni, percentualeSconto);
            ListaScontiOmbrellone.Add(scontoOmbrellone);
            string query = $"INSERT INTO sconto_ombrelloni (Numero_giorni, Sconto_corrispondente) VALUES ({numeroGiorni}, {percentualeSconto})";
            DBConnect dbConnect = new DBConnect();
            dbConnect.Insert(query);
            AggiuntoSconto?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviSconto(int numeroGiorni)
        {
            for (int i = ListaScontiOmbrellone.Count - 1; i >= 0; i--)
            {
                if (ListaScontiOmbrellone[i].NumeroGiorni == numeroGiorni)
                {
                    ListaScontiOmbrellone.RemoveAt(i);
                }
            }
            RimossoSconto?.Invoke(this, EventArgs.Empty);
        }

        public List<string> GetSconti()
        {
            List<string> toReturn = new List<string>();
            foreach (ScontoOmbrellone scontoOmbrellone in ListaScontiOmbrellone)
            {
                toReturn.Add(scontoOmbrellone.ToString());
            }
            return toReturn;
        }


        #endregion

        public List<string> GetEmails()
        {
            List<string> toReturn = new List<string>();
            foreach (Cliente cliente in ListaClienti)
            {
                if (cliente.Email != "")
                {
                    toReturn.Add(cliente.Email);
                }
            }
            return toReturn;
        }
    }
}
