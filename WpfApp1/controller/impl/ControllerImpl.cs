using MySql.Data.MySqlClient;
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
        public ObservableCollection<Documento> ListaDocumenti { get; set; }

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
            ListaDocumenti = new ObservableCollection<Documento>();
            LoadClientiFromDB();
            LoadPiattiFromDB();
            LoadOspitiFromDB();
            LoadMenuFromDB();
            LoadEventiFromDB();
            LoadPrenotazioniTavoloFromDB();
            LoadPrenotazioniOmbrelloneFromDB();
            SetRighe();
        }

        #region Ombrelloni

        public void AggiungiOmbrellone(int numeroRiga, int numeroColonna)
        {
            DBConnect dbConnect2 = new DBConnect();
            bool isRigaPresente = false;
            string query2 = "SELECT Numero_riga FROM riga;";
            List<MySqlParameter> parameters2 = new List<MySqlParameter>();
            DataTable dataTable2 = dbConnect2.Select(query2, parameters2);
            foreach (DataRow row in dataTable2.Rows)
            {
                int nRiga = int.Parse(row["Numero_riga"].ToString());
                if (nRiga == numeroRiga)
                {
                    isRigaPresente = true;
                }
            }
            if (!isRigaPresente)
            {
                DBConnect dbConnect3 = new DBConnect();
                string query3 = $"INSERT INTO Riga VALUES(@numeroRiga);";
                List<MySqlParameter> parameters3 = new List<MySqlParameter>
                {
                    new MySqlParameter("@numeroRiga", MySqlDbType.Int32) { Value = numeroRiga }
                };
                _ = dbConnect3.Insert(query3, parameters3);
            }
            Ombrellone ombrellone = new Ombrellone(numeroRiga, numeroColonna);
            ListaOmbrelloni.Add(ombrellone);
            string query = $"INSERT INTO Ombrellone (Numero_riga, Numero_colonna) VALUES (@numeroRiga, @numeroColonna)";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@numeroRiga", MySqlDbType.Int32) { Value = numeroRiga },
                new MySqlParameter("@numeroColonna", MySqlDbType.Int32) { Value = numeroColonna }
            };
            DBConnect dbConnect = new DBConnect();
            _ = dbConnect.Insert(query, parameters);
            AggiuntoOmbrellone?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviOmbrellone(int numeroRiga, int numeroColonna)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    string query = $"DELETE FROM ombrellone WHERE Numero_riga = @numeroRiga AND Numero_colonna = @numeroColonna";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@numeroRiga", MySqlDbType.Int32) { Value = numeroRiga },
                        new MySqlParameter("@numeroColonna", MySqlDbType.Int32) { Value = numeroRiga }
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
                    ListaOmbrelloni.RemoveAt(i);
                }
            }
            RimossoOmbrellone?.Invoke(this, EventArgs.Empty);
        }

        public void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInizio, DateTime dataFine, string codiceFiscalePrenotante, int numeroLettiniAggiuntivi)
        {
            for (int i = ListaOmbrelloni.Count - 1; i >= 0; i--)
            {
                if (ListaOmbrelloni[i].NumeroRiga == numeroRiga && ListaOmbrelloni[i].NumeroColonna == numeroColonna)
                {
                    PrenotazioneOmbrellone prenotazione = new PrenotazioneOmbrellone(dataInizio, dataFine, numeroRiga, numeroColonna, codiceFiscalePrenotante, numeroLettiniAggiuntivi);
                    string query = $"INSERT INTO prenotazione_ombrellone (Data_inizio, Data_fine, Numero_lettini_aggiuntivi, Codice_fiscale, Numero_riga, Numero_colonna) VALUES (@dataInizio, @dataFine, @numeroLettiniAggiuntivi, @cf, @numeroRiga, @numeroColonna)";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                        new MySqlParameter("@dataFine", MySqlDbType.Date) { Value = dataFine },
                        new MySqlParameter("@numeroRiga", MySqlDbType.Int32) { Value = numeroRiga },
                        new MySqlParameter("@cf", MySqlDbType.VarChar, 16) { Value = codiceFiscalePrenotante },
                        new MySqlParameter("@numeroLettiniAggiuntivi", MySqlDbType.Int32) { Value = numeroLettiniAggiuntivi },
                        new MySqlParameter("@numeroColonna", MySqlDbType.Int32) { Value = numeroColonna },
                    };
                    DBConnect dbConnect = new DBConnect();
                    _ = dbConnect.Insert(query, parameters);
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
                    string query = $"DELETE FROM prenotazione_ombrellone WHERE Data_inizio = @dataInizio AND Numero_riga = @numeroRiga AND Numero_colonna = @numeroColonna";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                        new MySqlParameter("@numeroRiga", MySqlDbType.Int32) { Value = numeroRiga },
                        new MySqlParameter("@numeroColonna", MySqlDbType.Int32) { Value = numeroRiga }
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
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
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
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

        public void LoadOmbrelloniFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM ombrellone;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                int nRiga = int.Parse(row["Numero_riga"].ToString());
                int nColonna = int.Parse(row["Numero_colonna"].ToString());
                ListaOmbrelloni.Add(new Ombrellone(nRiga, nColonna));
            }
        }

        public void LoadPrenotazioniOmbrelloneFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM prenotazione_ombrellone;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime dataInizio = DateTime.Parse(row["Data_inizio"].ToString());
                DateTime dataFine = DateTime.Parse(row["Data_fine"].ToString());
                int nLettiniAggiuntivi = int.Parse(row["Numero_lettini_aggiuntivi"].ToString());
                string cf = row["Codice_fiscale"].ToString();
                int nRiga = int.Parse(row["Numero_riga"].ToString());
                int nColonna = int.Parse(row["Numero_colonna"].ToString());
                PrenotazioneOmbrellone prenotazioneOmbrellone = new PrenotazioneOmbrellone(dataInizio, dataFine, nRiga, nColonna, cf, nLettiniAggiuntivi);
                ListaPrenotazioniOmbrelloni.Add(prenotazioneOmbrellone);
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
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int nRiga = int.Parse(row["Numero_riga"].ToString());
                int nColonna = int.Parse(row["Numero_colonna"].ToString());
                return (nRiga, nColonna);
            }
            return (0, 0);
        }

        public void LoadDocumentiFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM documento;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                string codiceDocumento = row["Codice_documento"].ToString();
                string cf = row["Codice_fiscale"].ToString();
                string tipo = row["Tipo"].ToString();
                TipoDocumento tipoDocumento = (TipoDocumento)Enum.Parse(typeof(TipoDocumento), tipo);
                ListaDocumenti.Add(new Documento(codiceDocumento, cf, tipoDocumento));
            }
        }

        public List<string> GetDocumenti()
        {
            LoadDocumentiFromDB();
            List<string> toReturn = new List<string>();
            foreach (Documento documento in ListaDocumenti)
            {
                toReturn.Add(documento.ToString());
            }
            return toReturn;
        }

        #endregion

        #region Tavoli

        public void AggiungiTavolo(int idTavolo, int numeroPosti)
        {
            Tavolo tavolo = new Tavolo(idTavolo, numeroPosti);
            ListaTavoli.Add(tavolo);
            string query = $"INSERT INTO Tavolo (Id_tavolo, Numero_posti) VALUES (@idTavolo, @numeroPosti)";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@idTavolo", MySqlDbType.Int32) { Value = idTavolo },
                new MySqlParameter("@numeroPosti", MySqlDbType.Int32) { Value = numeroPosti }
            };
            DBConnect dbConnect = new DBConnect();
            _ = dbConnect.Insert(query, parameters);
            AggiuntoTavolo?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviTavolo(int idTavolo)
        {
            for (int i = ListaTavoli.Count - 1; i >= 0; i--)
            {
                if (ListaTavoli[i].IdTavolo == idTavolo)
                {
                    string query = $"DELETE FROM tavolo WHERE Id_tavolo = @idTavolo";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@idTavolo", MySqlDbType.Int32) { Value = idTavolo }
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
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
                    string query = $"INSERT INTO prenotazione_tavolo (Id_tavolo, Data, Pasto, Numero_persone_prenotanti, Codice_fiscale) VALUES (@idTavolo, @data, @pasto, @numeroPersonePrenotanti, @cf)";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@idTavolo", MySqlDbType.Int32) { Value = idTavolo },
                        new MySqlParameter("@data", MySqlDbType.Date) { Value = data },
                        new MySqlParameter("@pasto", MySqlDbType.VarChar, 50) { Value = pasto },
                        new MySqlParameter("@numeroPersonePrenotanti", MySqlDbType.Int32) { Value = numeroPersonePrenotanti },
                        new MySqlParameter("@cf", MySqlDbType.VarChar, 16) { Value = codiceFiscalePrenotante }
                    };
                    DBConnect dbConnect = new DBConnect();
                    _ = dbConnect.Insert(query, parameters);
                }
            }
        }

        public void LoadPrenotazioniTavoloFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM prenotazione_tavolo;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                int idTavolo = int.Parse(row["Id_tavolo"].ToString());
                DateTime data = DateTime.Parse(row["Data"].ToString());
                string pasto = row["Pasto"].ToString();
                int numeroPersonePrenotanti = int.Parse(row["Numero_persone_prenotanti"].ToString());
                string cf = row["Codice_fiscale"].ToString();
                PrenotazioneTavolo prenotazioneTavolo = new PrenotazioneTavolo(data, (Pasto)Enum.Parse(typeof(Pasto), pasto), idTavolo, cf, numeroPersonePrenotanti);
                ListaPrenotazioniTavoli.Add(prenotazioneTavolo);
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
                    string query = $"DELETE FROM prenotazione_tavolo WHERE Id_tavolo = @idTavolo AND Data = @data AND Pasto = @pasto";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@idTavolo", MySqlDbType.Int32) { Value = idTavolo },
                        new MySqlParameter("@data", MySqlDbType.Date) { Value = data },
                        new MySqlParameter("@pasto", MySqlDbType.VarChar, 50 ) { Value = pasto }
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
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
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
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
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
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
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
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
            string query = $"INSERT INTO Piatto (Nome, Prezzo, Descrizione) VALUES( @nome, @prezzo, @descrizione);";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@prezzo", MySqlDbType.Double) { Value = prezzo },
                new MySqlParameter("@nome", MySqlDbType.VarChar, 50) { Value = nome },
                new MySqlParameter("@descrizione", MySqlDbType.VarChar, 300) { Value = descrizione }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Il piatto non è stato inserito perché già esistente un piatto con lo stesso nome");
            }
            AggiuntoPiatto?.Invoke(this, EventArgs.Empty);
        }

        private void AggiungiPiattoInElencoPiatti(int idMenu, string nome)
        {
            string query = $"INSERT INTO elencoPiatti (Nome, Id_Menu) VALUES(@nome, @idMenu)";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@idMenu", MySqlDbType.Int32) { Value = idMenu },
                new MySqlParameter("@nome", MySqlDbType.VarChar, 50) { Value = nome }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Il menù {idMenu} contiene già il piatto {nome}");
            }
        }

        public void RimuoviPiatto(string nome)
        {
            for (int i = ListaPiatti.Count - 1; i >= 0; i--)
            {
                if (ListaPiatti[i].Nome == nome)
                {
                    string query = $"DELETE FROM piatto WHERE Nome = @nome";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@nome", MySqlDbType.VarChar, 50) { Value = nome }
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
                    ListaPiatti.RemoveAt(i);
                }
            }
            RimossoPiatto?.Invoke(this, EventArgs.Empty);
        }

        public void LoadPiattiFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM piatto;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                string nome = row["Nome"].ToString();
                double prezzo = double.Parse(row["Prezzo"].ToString());
                string descrizione = row["Descrizione"].ToString();
                ListaPiatti.Add(new Piatto(nome, prezzo, descrizione));
            }
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
                string query = $"INSERT INTO Menu (Id_menu, Prezzo) VALUES(@idMenu, @prezzo);";
                List<MySqlParameter> parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("@idMenu", MySqlDbType.Int32) { Value = idMenu },
                    new MySqlParameter("@prezzo", MySqlDbType.Double) { Value = prezzo }
                };
                DBConnect dbConnect = new DBConnect();
                int rowsAffected = dbConnect.Insert(query, parameters);
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Il menù non è stato inserito perché già esistente un menù con lo stesso id");
                }
                foreach (Piatto piatto in listaPiatti)
                {
                    AggiungiPiattoInElencoPiatti(idMenu, piatto.Nome);
                }
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
                    string query = $"DELETE FROM menu WHERE Id_menu = @idMenu";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@idMenu", MySqlDbType.Int32) { Value = idMenu }
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
                    string query2 = $"DELETE FROM elencoPiatti WHERE Id_menu = @idMenu";
                    DBConnect dbConnect2 = new DBConnect();
                    dbConnect2.Delete(query2, parameters);
                    ListaMenu.RemoveAt(i);
                }
            }
            RimossoMenu?.Invoke(this, EventArgs.Empty);
        }

        public void LoadMenuFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM menu;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                int id_menu = int.Parse(row["Id_menu"].ToString());
                double prezzo = double.Parse(row["Prezzo"].ToString());
                List<Piatto> elencoPiatti = new List<Piatto>();
                DBConnect dbConnect2 = new DBConnect();
                string query2 = $"SELECT * FROM elencoPiatti WHERE Id_menu = {id_menu};";
                List<MySqlParameter> parameters2 = new List<MySqlParameter>
                {
                    new MySqlParameter("@idMenu", MySqlDbType.Int32) { Value = id_menu }
                };
                DataTable dataTable2 = dbConnect2.Select(query2, parameters2);
                foreach (DataRow row2 in dataTable2.Rows)
                {
                    int id_menuElencoPiatti = int.Parse(row2["Id_menu"].ToString());
                    string nomePiatto = row2["Nome"].ToString();
                    DBConnect dbConnect3 = new DBConnect();
                    string query3 = $"SELECT * FROM Piatto WHERE Nome = @nomePiatto;";
                    List<MySqlParameter> parameters3 = new List<MySqlParameter>
                    {
                        new MySqlParameter("@nomePiatto", MySqlDbType.VarChar, 50) { Value = nomePiatto }
                    };
                    DataTable dataTable3 = dbConnect3.Select(query3, parameters3);

                    if (dataTable3.Rows.Count > 0)
                    {
                        DataRow row3 = dataTable3.Rows[0];
                        string nome = row3["Nome"].ToString();
                        double prezzoPiatto = double.Parse(row3["Prezzo"].ToString());
                        string descrizione = row3["Descrizione"].ToString();
                        elencoPiatti.Add(new Piatto(nome, prezzoPiatto, descrizione));
                    }
                }
                ListaMenu.Add(new Menu(id_menu, elencoPiatti, prezzo));
            }
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

        public int GetLastMenuId()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT MAX(Id_menu) AS max_id FROM menu;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);

            return dataTable.Rows.Count > 0 && dataTable.Rows[0]["max_id"] != DBNull.Value
                ? int.TryParse(dataTable.Rows[0]["max_id"].ToString(), out int maxId) ? maxId : 0
                : 0;
        }

        public void AggiungiOrdine(int idOrdine, DateTime data, string pasto, int idTavolo, List<int> idMenuOrdinati, List<string> nomiPiattiOrdinati)
        {
            List<Piatto> piatti = new List<Piatto>();
            List<Menu> menus = new List<Menu>();
            int indexPrenotazioneTavolo = 0;
            for (int i = idMenuOrdinati.Count - 1; i >= 0; i--)
            {
                for (int j = ListaMenu.Count - 1; j >= 0; j--)
                {
                    if (idMenuOrdinati[i] == ListaMenu[j].IdMenu)
                    {
                        menus.Add(ListaMenu[j]);
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
            Ordine ordine = new Ordine(idOrdine, ListaPrenotazioniTavoli[indexPrenotazioneTavolo], piatti, menus);
            string query = $"INSERT INTO ordine (Id_ordine, Id_tavolo, Data, Pasto) VALUES(@idOrdine, @idTavolo, @data, @pasto);";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@idOrdine", MySqlDbType.Int32) { Value = idOrdine },
                new MySqlParameter("@idTavolo", MySqlDbType.Int32) { Value = idTavolo },
                new MySqlParameter("@data", MySqlDbType.Date) { Value = data },
                new MySqlParameter("@pasto", MySqlDbType.VarChar, 50) { Value = pasto },
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Esiste già un ordine con questo id: {idOrdine}");
            }
            ListaOrdini.Add(ordine);
        }

        public void AggiungiMenuInContenenzaMenu(int idMenu, int idOrdine, int quantita)
        {
            string query = $"INSERT INTO contenenza_menu (Id_menu, Id_ordine, quantità) VALUES(@idMenu, @idOrdine, @quantita);";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@idMenu", MySqlDbType.Int32) { Value = idMenu },
                new MySqlParameter("@idOrdine", MySqlDbType.Int32) { Value = idOrdine },
                new MySqlParameter("@quantita", MySqlDbType.Int32) { Value = quantita }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Il menù {idMenu} è già contenuto nell'ordine {idOrdine}");
            }
        }

        public void AggiungiPiattiInContenenzaPiatti(string nome, int idOrdine, int quantita)
        {
            string query = $"INSERT INTO contenenza_piatti (Nome, Id_ordine, quantità) VALUES(@nome, @idOrdine, @quantita);";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@nome", MySqlDbType.VarChar, 50) { Value = nome },
                new MySqlParameter("@idOrdine", MySqlDbType.Int32) { Value = idOrdine },
                new MySqlParameter("@quantita", MySqlDbType.Int32) { Value = quantita }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Il piatto {nome} è già contenuto nell'ordine {idOrdine}");
            }
        }

        public int GetNumeroOrdini()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT COUNT(Id_ordine) AS numero_ordini FROM ordine;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int nTavoli = int.Parse(row["numero_ordini"].ToString());
                return nTavoli;
            }
            return 0;
        }

        public int GetLastIdOrdine()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT MAX(Id_ordine) AS max_id FROM ordine;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            return dataTable.Rows.Count > 0 && dataTable.Rows[0]["max_id"] != DBNull.Value
                ? int.TryParse(dataTable.Rows[0]["max_id"].ToString(), out int maxId) ? maxId : 0
                : 0;
        }

        #endregion

        #region Cliente
        public void AggiungiCliente(string nome, string cognome, string numeroTelefono, string città, string via, int civico, string email, string codiceFiscale)
        {
            Cliente cliente = new Cliente(città, via, civico, email, codiceFiscale, nome, cognome, numeroTelefono);
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@cf", MySqlDbType.VarChar, 16) { Value = codiceFiscale},
                new MySqlParameter("@nome", MySqlDbType.VarChar, 50) { Value = nome },
                new MySqlParameter("@cognome", MySqlDbType.VarChar, 50) { Value = cognome },
                new MySqlParameter("@numeroTelefono", MySqlDbType.VarChar, 15) { Value = numeroTelefono },
                new MySqlParameter("@città", MySqlDbType.VarChar, 50) { Value = città },
                new MySqlParameter("@via", MySqlDbType.VarChar, 50) { Value = via },
                new MySqlParameter("@civico", MySqlDbType.Int32) { Value = civico },
                new MySqlParameter("@email", MySqlDbType.VarChar, 255) { Value = email },
            };
            string query = email.Equals("")
                ? $"INSERT INTO Cliente (Codice_fiscale, Nome, Cognome, Numero_telefono, Ind_Citta, Ind_Via, Ind_Civico, Email) VALUES(@cf, @nome, @cognome, @numeroTelefono, @città, @via, @civico, NULL);"
                : $"INSERT INTO Cliente (Codice_fiscale, Nome, Cognome, Numero_telefono, Ind_Citta, Ind_Via, Ind_Civico, Email) VALUES(@cf, @nome, @cognome, @numeroTelefono, @città, @via, @civico, @email);";
            ListaClienti.Add(cliente);
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Il cliente non è stato inserito perché già esistente");
            }
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

        public void AggiungiDocumento(string codiceDocumento, string codiceFiscale, string tipo)
        {
            TipoDocumento tipoDocumento = (TipoDocumento)Enum.Parse(typeof(TipoDocumento), tipo);
            Documento documento = new Documento(codiceDocumento, codiceFiscale, tipoDocumento);
            ListaDocumenti.Add(documento);
            string query = $"INSERT INTO Documento (Codice_documento, Codice_fiscale, Tipo) VALUES (@codiceDocumento, @cf, @tipo)";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@codiceDocumento", MySqlDbType.VarChar, 50) { Value = codiceDocumento},
                new MySqlParameter("@cf", MySqlDbType.VarChar, 16) { Value = codiceFiscale },
                new MySqlParameter("@tipo", MySqlDbType.VarChar, 50) { Value = tipoDocumento },
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Il documento non è stato inserito perché già esistente");
            }
        }

        private List<string> GetClientiConEmail()
        {
            DBConnect dbConnect = new DBConnect();
            List<string> clientiConMail = new List<string>();
            string query = "select Codice_fiscale from cliente where email is not null;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                string cf = row["Codice_fiscale"].ToString();
                clientiConMail.Add(cf);
            }
            return clientiConMail;
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
            string query = $"INSERT INTO Evento (Titolo, Data, Orario_inizio, Descrizione, Costo_ingresso) VALUES(@titolo, @data, @orarioInizio, @descrizione, @costoIngresso);";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@titolo", MySqlDbType.VarChar, 50) { Value = titolo },
                new MySqlParameter("@data", MySqlDbType.Date) { Value = data },
                new MySqlParameter("@orarioInizio", MySqlDbType.Time) { Value = orario },
                new MySqlParameter("@descrizione", MySqlDbType.VarChar, 300) { Value = descrizione },
                new MySqlParameter("@costoIngresso", MySqlDbType.Double) { Value = costoIngrezzo }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Evento non inserito perché già presente evento con quella data e titolo");
            }
            foreach (Ospite ospite in ospiti)
            {
                AggiungiInConvocazione(titolo, data, ospite.CodiceFiscale);
            }
            List<string> clientiDaAvvisare = GetClientiConEmail();
            for (int i = 0; i < clientiDaAvvisare.Count; i++)
            {
                AggiungiInAvviso(titolo, data, clientiDaAvvisare[i]);
            }
            ListaEventi.Add(evento);
            AggiuntoEvento?.Invoke(this, EventArgs.Empty);
        }

        private void AggiungiInAvviso(string titolo, DateTime data, string cf)
        {
            string query = $"INSERT INTO avviso (Titolo, Data, Codice_fiscale) VALUES (@titolo, @data, @cf)";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@titolo", MySqlDbType.VarChar, 50) { Value = titolo },
                new MySqlParameter("@cf", MySqlDbType.VarChar, 50) { Value = cf },
                new MySqlParameter("@data", MySqlDbType.Date) { Value = data }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Esiste già la convocazione dell'evento '{titolo}' alla data '{data:yyyy-MM-dd}' di '{cf}'");
            }
        }

        public void RimuoviEvento(string titolo, DateTime data)
        {
            for (int i = ListaEventi.Count - 1; i >= 0; i--)
            {
                if (ListaEventi[i].Titolo == titolo && ListaEventi[i].Data == data)
                {
                    string query = $"DELETE FROM evento WHERE Titolo = @titolo AND Data = @data";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@titolo", MySqlDbType.VarChar, 50) { Value = titolo },
                        new MySqlParameter("@data", MySqlDbType.Date) { Value = data }
                    };
                    RimuoviDaConvocazione(titolo, data);
                    RimuoviDaAvviso(titolo, data);
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
                    ListaEventi.RemoveAt(i);
                }
            }
            RimossoEvento?.Invoke(this, EventArgs.Empty);
        }

        private void RimuoviDaAvviso(string titolo, DateTime data)
        {
            string query = $"DELETE FROM avviso WHERE Titolo = @titolo AND Data = @data";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@titolo", MySqlDbType.VarChar, 50) { Value = titolo },
                new MySqlParameter("@data", MySqlDbType.Date) { Value = data }
            };
            DBConnect dbConnect = new DBConnect();
            dbConnect.Delete(query, parameters);
        }

        private void RimuoviDaConvocazione(string titolo, DateTime data)
        {
            string query = $"DELETE FROM convocazione WHERE Titolo = @titolo AND Data = @data";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@titolo", MySqlDbType.VarChar, 50) { Value = titolo },
                new MySqlParameter("@data", MySqlDbType.Date) { Value = data }
            };
            DBConnect dbConnect = new DBConnect();
            dbConnect.Delete(query, parameters);
        }

        public void LoadEventiFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM evento;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                string titolo = row["Titolo"].ToString();
                DateTime data = DateTime.Parse(row["Data"].ToString());
                TimeSpan orarioInizio = TimeSpan.Parse(row["Orario_inizio"].ToString());
                string descrizione = row["Descrizione"].ToString();
                double costoIngresso = double.Parse(row["Costo_ingresso"].ToString());
                List<Ospite> ospiti = new List<Ospite>();
                DBConnect dbConnect2 = new DBConnect();
                string query2 = $"SELECT * FROM convocazione WHERE Titolo = @titolo AND Data = @data;";
                List<MySqlParameter> parameters2 = new List<MySqlParameter>
                {
                    new MySqlParameter("@titolo", MySqlDbType.VarChar, 50) { Value = titolo },
                    new MySqlParameter("@data", MySqlDbType.Date) { Value = data }
                };
                DataTable dataTable2 = dbConnect2.Select(query2, parameters2);
                foreach (DataRow row2 in dataTable2.Rows)
                {
                    string cf = row2["Codice_fiscale"].ToString();
                    DBConnect dbConnect3 = new DBConnect();
                    string query3 = $"SELECT * FROM Ospite WHERE Codice_fiscale = @cf;";
                    List<MySqlParameter> parameters3 = new List<MySqlParameter>
                    {
                        new MySqlParameter("@cf", MySqlDbType.VarChar, 1) { Value = cf },
                    };
                    DataTable dataTable3 = dbConnect3.Select(query3, parameters3);
                    if (dataTable3.Rows.Count > 0)
                    {
                        DataRow row3 = dataTable3.Rows[0];
                        string nome = row3["Nome"].ToString();
                        string cognome = row3["Cognome"].ToString();
                        string numeroTelefono = row3["Numero_telefono"].ToString();
                        string nickname = row3["Nickname"].ToString();
                        ospiti.Add(new Ospite(cf, nome, cognome, numeroTelefono, nickname));
                    }
                }
                ListaEventi.Add(new Evento(titolo, data, orarioInizio, descrizione, costoIngresso, ospiti));
            }
        }

        public void LoadOspitiFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM ospite;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                string cf = row["Codice_fiscale"].ToString();
                string nome = row["Nome"].ToString();
                string cognome = row["Cognome"].ToString();
                string numeroTelefono = row["Numero_telefono"].ToString();
                string nickname = row["Nickname"].ToString();
                ListaOspiti.Add(new Ospite(cf, nome, cognome, numeroTelefono, nickname));
            }
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
            string query = $"INSERT INTO Ospite (Codice_fiscale, Nome, Cognome, Numero_telefono, Nickname) VALUES (@cf, @nome, @cognome, @numeroTelefono, @nickname);";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@cf", MySqlDbType.VarChar, 16) { Value = codiceFiscale },
                new MySqlParameter("@cognome", MySqlDbType.VarChar, 50) { Value = cognome },
                new MySqlParameter("@nome", MySqlDbType.VarChar, 50) { Value = nome },
                new MySqlParameter("@numeroTelefono", MySqlDbType.VarChar, 50) { Value = numeroTelefono },
                new MySqlParameter("@nickname", MySqlDbType.VarChar, 50) { Value = nickname }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Ospite non inserito perché già presente un ospite con lo stesso codice fiscale");
            }
            ListaOspiti.Add(ospite);
            AggiuntoOspite?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviOspite(string codiceFiscale)
        {
            for (int i = ListaOspiti.Count - 1; i >= 0; i--)
            {
                if (ListaOspiti[i].CodiceFiscale == codiceFiscale)
                {
                    string query = $"DELETE FROM Ospite WHERE Codice_fiscale = @cf";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@cf", MySqlDbType.VarChar, 16) { Value = codiceFiscale },
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
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

        public void LoadClientiFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM cliente;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                string cf = row["Codice_fiscale"].ToString();
                string nome = row["Nome"].ToString();
                string cognome = row["Cognome"].ToString();
                string numeroTelefono = row["Numero_telefono"].ToString();
                string citta = row["Ind_Citta"].ToString();
                string via = row["Ind_Via"].ToString();
                int civico = int.Parse(row["Ind_Civico"].ToString());
                string email = row["Email"].ToString();
                ListaClienti.Add(new Cliente(citta, via, civico, email, cf, nome, cognome, numeroTelefono));
            }
        }

        private void AggiungiInConvocazione(string titolo, DateTime data, string codiceFiscale)
        {
            string query = $"INSERT INTO convocazione (Titolo, Data, Codice_fiscale) VALUES(@titolo, @data, @cf)";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@titolo", MySqlDbType.VarChar, 50) { Value = titolo },
                new MySqlParameter("@data", MySqlDbType.Date) { Value = data },
                new MySqlParameter("@cf", MySqlDbType.VarChar, 16) { Value = codiceFiscale }
            };
            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Esiste già la convocazione dell'evento {titolo} alla data {data:yyyy-MM-dd} di {codiceFiscale}");
            }
        }

        #endregion

        #region Sconti

        public void AggiungiSconto(int numeroGiorni, double percentualeSconto)
        {
            ScontoOmbrellone scontoOmbrellone = new ScontoOmbrellone(numeroGiorni, percentualeSconto);
            ListaScontiOmbrellone.Add(scontoOmbrellone);
            string query = $"INSERT INTO sconto_ombrelloni (Numero_giorni, Sconto_corrispondente) VALUES(@numeroGiorni, @percentualeSconto)";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@numeroGiorni", MySqlDbType.Int32) { Value = numeroGiorni },
                new MySqlParameter("@percentualeSconto", MySqlDbType.Double) { Value = percentualeSconto }
            };

            DBConnect dbConnect = new DBConnect();
            int rowsAffected = dbConnect.Insert(query, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Esiste già uno sconto per questo numero di giorni");
            }
            AggiuntoSconto?.Invoke(this, EventArgs.Empty);
        }

        public void RimuoviSconto(int numeroGiorni)
        {
            for (int i = ListaScontiOmbrellone.Count - 1; i >= 0; i--)
            {
                if (ListaScontiOmbrellone[i].NumeroGiorni == numeroGiorni)
                {
                    string query = $"DELETE FROM sconto_ombrelloni WHERE Numero_giorni = @numeroGiorni";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@numeroGiorni", MySqlDbType.Int32) { Value = numeroGiorni }
                    };
                    DBConnect dbConnect = new DBConnect();
                    dbConnect.Delete(query, parameters);
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

        public void LoadScontiFromDB()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT * FROM sconto_ombrelloni;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                int nGiorni = int.Parse(row["Numero_giorni"].ToString());
                double percentualeSconto = double.Parse(row["Sconto_corrispondente"].ToString());
                ListaScontiOmbrellone.Add(new ScontoOmbrellone(nGiorni, percentualeSconto));
            }
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

        public int GetNumeroRighe()
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT COUNT(Numero_riga) AS nRighe FROM riga;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int nRighe = int.Parse(row["nRighe"].ToString());
                return nRighe;
            }
            return 0;
        }

        #region Prezzi
        public bool CheckPriceAreSetted()
        {
            DBConnect dbConnect = new DBConnect();
            string queryPrezziOmbrelloni = "SELECT COUNT(*) AS nPrezziOmbrelloni FROM prezzo_ombrellone;";
            int nPrezziOmbrelloni = 0, nPrezziLettini = 0;
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(queryPrezziOmbrelloni, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                nPrezziOmbrelloni = int.Parse(row["nPrezziOmbrelloni"].ToString());
            }

            string queryPrezziLettini = "SELECT COUNT(*) AS nPrezziLettini FROM prezzo_lettino;";
            List<MySqlParameter> parameters2 = new List<MySqlParameter>();
            DataTable dataTable2 = dbConnect.Select(queryPrezziLettini, parameters2);
            if (dataTable2.Rows.Count > 0)
            {
                DataRow row = dataTable2.Rows[0];
                nPrezziLettini = int.Parse(row["nPrezziLettini"].ToString());
            }
            return nPrezziLettini > 0 && nPrezziOmbrelloni > 0;
        }

        public void SetRighe()
        {
            string query = $"INSERT INTO Riga (numero_riga) SELECT DISTINCT Numero_riga FROM ombrellone AS o WHERE NOT EXISTS( SELECT 1 FROM Riga AS r WHERE r.numero_riga = o.Numero_riga) ORDER BY Numero_riga DESC;";
            DBConnect dbConnect = new DBConnect();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            _ = dbConnect.Insert(query, parameters);
        }

        public void AggiungiPrezziOmbrelloni(int nRiga, string periodo, double prezzo)
        {
            string query = $"INSERT INTO prezzo_ombrellone (Numero_riga, Periodo, Prezzo_giornaliero) VALUES (@nRiga, @periodo, @prezzo) ON DUPLICATE KEY UPDATE Prezzo_giornaliero = VALUES(Prezzo_giornaliero); ";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@nRiga", MySqlDbType.Int32) { Value = nRiga },
                new MySqlParameter("@periodo", MySqlDbType.VarChar, 50) { Value = periodo },
                new MySqlParameter("@prezzo", MySqlDbType.Double) { Value = prezzo }
            };
            DBConnect dbConnect = new DBConnect();
            _ = dbConnect.Insert(query, parameters);
        }

        public void AggiungiPrezziLettini(int nRiga, string periodo, double prezzo)
        {
            string query = $"INSERT INTO prezzo_lettino (Periodo, Prezzo_giornaliero, Numero_riga) VALUES (@periodo, @prezzo, @nRiga) ON DUPLICATE KEY UPDATE Prezzo_giornaliero = VALUES(Prezzo_giornaliero); ";
            DBConnect dbConnect = new DBConnect();
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@periodo", MySqlDbType.VarChar, 50) { Value = periodo },
                new MySqlParameter("@prezzo", MySqlDbType.Double) { Value = prezzo },
                new MySqlParameter("@nRiga", MySqlDbType.Int32) { Value = nRiga }
            };
            _ = dbConnect.Insert(query, parameters);
        }

        public List<string> GetPrezziOmbrelloni()
        {
            List<string> toReturn = new List<string>();
            DBConnect dbConnect = new DBConnect();
            string[] periodi = { "BassaStagione", "AltaStagione" };
            int[] righe = { 1, 2, 3 };

            foreach (string periodo in periodi)
            {
                foreach (int riga in righe)
                {
                    string query = $"SELECT Prezzo_giornaliero FROM prezzo_ombrellone WHERE Periodo = @Periodo AND Numero_riga = @NumeroRiga;";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@Periodo", periodo),
                        new MySqlParameter("@NumeroRiga", riga)
                    };
                    DataTable dataTable = dbConnect.Select(query, parameters);
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];
                        string prezzo = row["Prezzo_giornaliero"].ToString();
                        toReturn.Add(prezzo);
                    }
                }
            }
            return toReturn;
        }

        public List<string> GetPrezziLettini()
        {
            List<string> toReturn = new List<string>();
            DBConnect dbConnect = new DBConnect();

            string[] periodi = { "BassaStagione", "AltaStagione" };
            int[] righe = { 1, 2, 3 };
            foreach (string periodo in periodi)
            {
                foreach (int riga in righe)
                {
                    string query = $"SELECT Prezzo_giornaliero FROM prezzo_lettino WHERE Periodo = @Periodo AND Numero_riga = @NumeroRiga;";
                    List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@Periodo", periodo),
                        new MySqlParameter("@NumeroRiga", riga)
                    };
                    DataTable dataTable = dbConnect.Select(query, parameters);
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];
                        string prezzo = row["Prezzo_giornaliero"].ToString();
                        toReturn.Add(prezzo);
                    }
                }
            }

            return toReturn;
        }

        #endregion

        #region Query per sezione bilanci
        public (int, int) GetIdMenuPiuOrdinato(DateTime dataInizio, DateTime dataFine) // restituisce l'id del menù più ordinato + la quantità
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"SELECT Id_menu, SUM(quantità) AS quantità_totale FROM(SELECT m.Id_menu, SUM(m.quantità) AS quantità FROM ordine o INNER JOIN contenenza_menu m ON o.Id_ordine = m.Id_ordine " +
                $"WHERE EXISTS(SELECT * FROM prenotazione_tavolo p WHERE p.Id_tavolo = o.Id_tavolo AND p.Data BETWEEN @dataInizio AND @dataFine AND p.Pasto = o.Pasto) " +
                $"GROUP BY m.Id_menu, o.Id_ordine) AS menu_ordine GROUP BY Id_menu ORDER BY quantità_totale DESC LIMIT 1;";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                new MySqlParameter("@dataFine", MySqlDbType.Date) { Value = dataFine },
            };
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int idMenu = int.Parse(row["Id_menu"].ToString());
                int quantità = int.Parse(row["quantità_totale"].ToString());
                return (idMenu, quantità);
            }
            return (0, 0);
        }

        public (int, int) GetIdMenuMenoOrdinato(DateTime dataInizio, DateTime dataFine)
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"SELECT Id_menu, SUM(quantità) AS quantità_totale FROM(SELECT m.Id_menu, SUM(m.quantità) AS quantità FROM ordine o INNER JOIN contenenza_menu m ON o.Id_ordine = m.Id_ordine " +
                $"WHERE EXISTS(SELECT * FROM prenotazione_tavolo p WHERE p.Id_tavolo = o.Id_tavolo AND p.Data BETWEEN @dataInizio AND @dataFine AND p.Pasto = o.Pasto) " +
                $"GROUP BY m.Id_menu, o.Id_ordine) AS menu_ordine GROUP BY Id_menu ORDER BY quantità_totale ASC LIMIT 1;";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                new MySqlParameter("@dataFine", MySqlDbType.Date) { Value = dataFine },
            };
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int idMenu = int.Parse(row["Id_menu"].ToString());
                int quantità = int.Parse(row["quantità_totale"].ToString());
                return (idMenu, quantità);
            }
            return (0, 0);
        }

        public (string, int) GetIdPiattoPiuOrdinato(DateTime dataInizio, DateTime dataFine)
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"SELECT cp.Nome, SUM(cp.quantità) AS quantità_totale FROM prenotazione_tavolo pt JOIN ordine o ON pt.Id_tavolo = o.Id_tavolo JOIN contenenza_piatti cp ON o.Id_ordine = cp.Id_ordine WHERE pt.Data BETWEEN @dataInizio AND @dataFine GROUP BY cp.Nome ORDER BY quantità_totale DESC LIMIT 1;";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                new MySqlParameter("@dataFine", MySqlDbType.Date) { Value = dataFine },
            };
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                string nome = row["Nome"].ToString();
                int quantità = int.Parse(row["quantità_totale"].ToString());
                return (nome, quantità);
            }
            return ("", 0);
        }

        public (string, int) GetIdPiattoMenoOrdinato(DateTime dataInizio, DateTime dataFine)
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"SELECT cp.Nome, SUM(cp.quantità) AS quantità_totale FROM prenotazione_tavolo pt JOIN ordine o ON pt.Id_tavolo = o.Id_tavolo JOIN contenenza_piatti cp ON o.Id_ordine = cp.Id_ordine WHERE pt.Data BETWEEN @dataInizio AND @dataFine GROUP BY cp.Nome ORDER BY quantità_totale ASC LIMIT 1;";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                new MySqlParameter("@dataFine", MySqlDbType.Date) { Value = dataFine },
            };
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                string piatto = row["Nome"].ToString();
                int quantità = int.Parse(row["quantità_totale"].ToString());
                return (piatto, quantità);
            }
            return ("", 0);
        }

        public double CalcolaPercentualeClientiConMail()
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"SELECT (SUM(Email IS NOT NULL) / COUNT(*)) * 100 AS Percentuale_mail FROM cliente;";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                double percentuale = double.Parse(row["Percentuale_mail"].ToString());
                return percentuale;
            }
            return 0;
        }

        public double CalcolaIncassiRistorante(DateTime dataInizio, DateTime dataFine)
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"SELECT SUM(Incassi_totali) AS Incassi_totali FROM(SELECT SUM((IF(cm.menu_prezzo_totale IS NULL, 0, cm.menu_prezzo_totale)) + (IF(cp.piatti_prezzo_totale IS NULL, 0, cp.piatti_prezzo_totale))) AS Incassi_totali FROM prenotazione_tavolo pt INNER JOIN ordine o ON pt.Id_tavolo = o.Id_tavolo AND pt.Data = o.Data AND pt.Pasto = o.Pasto LEFT JOIN(SELECT Id_ordine, SUM(m.Prezzo * quantità) AS menu_prezzo_totale FROM contenenza_menu cm LEFT JOIN menu m ON cm.Id_menu = m.Id_menu GROUP BY Id_ordine)" +
                $" AS cm ON o.Id_ordine = cm.Id_ordine LEFT JOIN(SELECT Id_ordine, SUM(p.Prezzo * quantità) AS piatti_prezzo_totale FROM contenenza_piatti cp LEFT JOIN piatto p ON cp.Nome = p.Nome GROUP BY Id_ordine) AS cp ON o.Id_ordine = cp.Id_ordine WHERE pt.Data BETWEEN @dataInizio AND @dataFine) AS totali;";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                new MySqlParameter("@dataFine", MySqlDbType.Date) { Value = dataFine },
            };
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                object incassiRistoranteObj = row["Incassi_totali"];
                if (incassiRistoranteObj != DBNull.Value)
                {
                    double incassiRistorante = Convert.ToDouble(incassiRistoranteObj);
                    return incassiRistorante;
                }
            }
            return 0;
        }

        public double CalcolaIncassiSpiaggia(DateTime dataInizio, DateTime dataFine)
        {
            DBConnect dbConnect = new DBConnect();
            string query = "SELECT (SELECT SUM((po.Prezzo_giornaliero * (DATEDIFF(p.Data_fine, p.Data_inizio) + 1)) * (1 - IFNULL((SELECT MAX(Sconto_corrispondente) FROM sconto_ombrelloni WHERE Numero_giorni <= DATEDIFF(p.Data_fine, p.Data_inizio) + 1), 0) / 100)) FROM prenotazione_ombrellone AS p JOIN prezzo_ombrellone AS po ON p.Numero_riga = po.Numero_riga WHERE p.Data_inizio >= @dataInizio AND p.Data_fine <= @dataFine AND( (po.Periodo = 'BassaStagione' AND MONTH(p.Data_inizio) IN(6, 9)) OR (po.Periodo = 'AltaStagione'" +
                " AND MONTH(p.Data_inizio) IN(7, 8)))) + (SELECT SUM((pl.Prezzo_giornaliero * p.Numero_lettini_aggiuntivi * (DATEDIFF(p.Data_fine, p.Data_inizio) + 1)) * (1 - IFNULL((SELECT MAX(Sconto_corrispondente) FROM sconto_ombrelloni WHERE Numero_giorni <= DATEDIFF(p.Data_fine, p.Data_inizio) + 1), 0) / 100)) FROM prenotazione_ombrellone AS p JOIN prezzo_lettino AS pl ON p.Numero_riga = pl.Numero_riga WHERE p.Data_inizio >= @dataInizio AND p.Data_fine <= @dataFine AND ((pl.Periodo = 'BassaStagione' AND MONTH(p.Data_inizio) IN(6, 9)) OR (pl.Periodo = 'AltaStagione' AND MONTH(p.Data_inizio) IN(7, 8)))) AS Incasso_totale;";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@dataInizio", MySqlDbType.Date) { Value = dataInizio },
                new MySqlParameter("@dataFine", MySqlDbType.Date) { Value = dataFine },
            };
            DataTable dataTable = dbConnect.Select(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                object incassiSpiaggiaObj = row["Incasso_totale"];
                if (incassiSpiaggiaObj != DBNull.Value)
                {
                    double incassiSpiaggia = Convert.ToDouble(incassiSpiaggiaObj);
                    return incassiSpiaggia;
                }
            }
            return 0;
        }

        #endregion
    }
}
