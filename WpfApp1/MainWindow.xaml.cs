using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using WpfApp1.view;


namespace WpfApp1
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private readonly MySqlConnection connection = null;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            string server = "localhost";
            string uid = "Roberto";
            string database = "StabilimentoBalneare";
            string pw = "progettoDb123";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + pw + ";";
            try
            {
                // Creazione della connessione al server MySQL
                connection = new MySqlConnection(connectionString);

                // Apertura della connessione
                connection.Open();
                // Creazione del database se non esiste già
                string createDatabaseQuery = $"CREATE DATABASE IF NOT EXISTS {database}";
                MySqlCommand createDatabase = new MySqlCommand(createDatabaseQuery, connection);
                int rowsAffected = createDatabase.ExecuteNonQuery(); // 0 se il database è stato creato, -1 se esisteva già

                string useDatabaseQuery = $"USE {database}";
                MySqlCommand useDatabase = new MySqlCommand(createDatabaseQuery, connection);
                _ = useDatabase.ExecuteNonQuery();

                // Creazione della tabella se non esiste già
                List<string> createTableQuery = new List<string> { "create table if not exists Avviso (Titolo Varchar(50) not null, Data date not null, Codice_fiscale Varchar(16) not null, constraint ID_Avviso_ID primary key (Codice_fiscale, Titolo, Data));",
                    "create table if not exists CLIENTE (Codice_fiscale Varchar(16) not null, Nome Varchar(50) not null, Cognome Varchar(50) not null, Numero_telefono Varchar(15) not null, Ind_Citta Varchar(50) not null, Ind_Via Varchar(50) not null, Ind_Civico int not null, Email Varchar(255), constraint ID_CLIENTE_ID primary key (Codice_fiscale));",
                    "create table if not exists DOCUMENTO (Codice_documento Varchar(50) not null, Codice_fiscale Varchar(16) not null, Tipo Varchar(50) not null, constraint ID_DOCUMENTO_ID primary key (Codice_documento), constraint SID_DOCUM_CLIEN_ID unique (Codice_fiscale));",
                    "create table if not exists EVENTO (Titolo Varchar(50) not null, Data date not null, Orario_inizio time not null, Descrizione Varchar(300) not null, Costo_ingresso double not null, constraint ID_EVENTO_ID primary key (Titolo, Data));",
                    "create table if not exists EVENTO (Titolo Varchar(50) not null, Data date not null, Orario_inizio time not null, Descrizione Varchar(300) not null, Costo_ingresso double not null, constraint ID_EVENTO_ID primary key (Titolo, Data));",
                    "create table if not exists MENU (Id_menu int not null, Prezzo double not null, constraint ID_MENU_ID primary key (Id_menu));",
                    "create table if not exists Contenenza_menu (Id_menu int not null, Id_ordine int not null, constraint ID_Contenenza_menu_ID primary key (Id_menu, Id_ordine));",
                    "create table if not exists Contenenza_piatti (Nome Varchar(50) not null, Id_ordine int not null, constraint ID_Contenenza_piatti_ID primary key (Nome, Id_ordine));",
                    "create table if not exists Convocazione (Titolo Varchar(50) not null, Data date not null, Codice_fiscale Varchar(16) not null, constraint ID_Convocazione_ID primary key (Titolo, Data, Codice_fiscale));",
                    "create table if not exists ElencoPiatti (Nome Varchar(50) not null, Id_menu int not null, constraint ID_ElencoPiatti_ID primary key (Nome, Id_menu));",
                    "create table if not exists OMBRELLONE (Numero_riga int not null, Numero_colonna int not null, constraint ID_OMBRELLONE_ID primary key (Numero_riga, Numero_colonna));",
                    "create table if not exists ORDINE (Id_ordine int not null, Id_tavolo int not null, Data date not null, Pasto Varchar(50) not null, constraint ID_ORDINE_ID primary key (Id_ordine));",
                    "create table if not exists OSPITE (Codice_fiscale Varchar(16) not null, Nome Varchar(50) not null, Cognome Varchar(50) not null, Numero_telefono Varchar(15) not null, Nickname Varchar(50) not null, constraint ID_OSPITE_ID primary key (Codice_fiscale));",
                    "create table if not exists PIATTO (Nome Varchar(50) not null, Prezzo double not null, Descrizione Varchar(300) not null, constraint ID_PIATTO_ID primary key (Nome));",
                    "create table if not exists PRENOTAZIONE_OMBRELLONE (Data_inizio date not null, Data_fine date not null, Numero_lettini_aggiuntivi int not null, Codice_fiscale Varchar(16) not null, Numero_riga int not null, Numero_colonna int not null, constraint ID_PRENOTAZIONE_OMBRELLONE_ID primary key (Data_inizio, Numero_riga, Numero_colonna));",
                    "create table if not exists PRENOTAZIONE_TAVOLO (Id_tavolo int not null, Data date not null, Pasto varchar(50) not null, Numero_persone_prenotanti int not null, Codice_fiscale Varchar(16) not null, constraint ID_PRENOTAZIONE_TAVOLO_ID primary key (Id_tavolo, Data, Pasto));",
                    "create table if not exists PREZZO_LETTINO (Periodo Varchar(50) not null, Prezzo_giornaliero double not null, Numero_riga int not null, constraint ID_PREZZO_LETTINO_ID primary key (Periodo));",
                    "create table if not exists PREZZO_OMBRELLONE (Numero_riga int not null, Periodo Varchar(50) not null, Prezzo_giornaliero double not null, constraint ID_PREZZO_OMBRELLONE_ID primary key (Numero_riga, Periodo));",
                    "create table if not exists RIGA (Numero_riga char(1) not null, constraint ID_RIGA_ID primary key (Numero_riga));",
                    "create table if not exists SCONTO_OMBRELLONI (Numero_giorni int not null, Sconto_corrispondente double not null, constraint ID_SCONTO_OMBRELLONI_ID primary key (Numero_giorni));",
                    "create table if not exists TAVOLO (Id_tavolo int not null, Numero_posti int not null, constraint ID_TAVOLO_ID primary key (Id_tavolo));",
                };

                List<string> alterTableQuery = new List<string> { "alter table Avviso add constraint REF_Avvis_CLIEN foreign key (Codice_fiscale) references CLIENTE;",
                    "alter table Avviso add constraint REF_Avvis_EVENT_FK foreign key (Titolo, Data) references EVENTO;",
                    "alter table DOCUMENTO add constraint SID_DOCUM_CLIEN_FK foreign key (Codice_fiscale) references CLIENTE;",
                    "alter table Contenenza_menu add constraint REF_Conte_ORDIN_1_FK foreign key (Id_ordine) references ORDINE;",
                    "alter table Contenenza_menu add constraint REF_Conte_MENU foreign key (Id_menu) references MENU;",
                    "alter table Contenenza_piatti add constraint REF_Conte_ORDIN_FK foreign key (Id_ordine) references ORDINE;",
                    "alter table Contenenza_piatti add constraint REF_Conte_PIATT foreign key (Nome) references PIATTO;",
                    "alter table Convocazione add constraint REF_Convo_OSPIT_FK foreign key (Codice_fiscale) references OSPITE;",
                    "alter table Convocazione add constraint REF_Convo_EVENT foreign key (Titolo, Data) references EVENTO;",
                    "alter table ElencoPiatti add constraint EQU_Elenc_MENU_FK foreign key (Id_menu) references MENU;",
                    "alter table ElencoPiatti add constraint EQU_Elenc_PIATT foreign key (Nome) references PIATTO;",
                    "alter table OMBRELLONE add constraint EQU_OMBRE_RIGA foreign key (Numero_riga) references RIGA;",
                    "alter table ORDINE add constraint EQU_ORDIN_PRENO_FK foreign key (Id_tavolo, Data, Pasto) references PRENOTAZIONE_TAVOLO;",
                    "alter table PRENOTAZIONE_OMBRELLONE add constraint EQU_PRENO_CLIEN_1_FK foreign key (Codice_fiscale) references CLIENTE;",
                    "alter table PRENOTAZIONE_OMBRELLONE add constraint EQU_PRENO_OMBRE_FK foreign key (Numero_riga, Numero_colonna) references OMBRELLONE;",
                    "alter table PRENOTAZIONE_TAVOLO add constraint EQU_PRENO_CLIEN_FK foreign key (Codice_fiscale) references CLIENTE;",
                    "alter table PRENOTAZIONE_TAVOLO add constraint EQU_PRENO_TAVOL foreign key (Id_tavolo) references TAVOLO;",
                    "alter table PREZZO__LETTINO add constraint EQU_PREZZ_RIGA_1_FK foreign key (Numero_riga) references RIGA;",
                    "alter table PREZZO_OMBRELLONE add constraint EQU_PREZZ_RIGA foreign key (Numero_riga) references RIGA;",
                };
                foreach (string query in createTableQuery)
                {
                    MySqlCommand createTable = new MySqlCommand(query, connection);
                    _ = createTable.ExecuteNonQuery();
                }
                if (rowsAffected == -1)
                {
                    foreach (string query in alterTableQuery)
                    {
                        MySqlCommand alterTable = new MySqlCommand(query, connection);
                        _ = alterTable.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Database e tabelle creati o già esistenti.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la creazione del database o delle tabelle: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }



        private void btnSpiaggia_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            WindowSpiaggia windowSpiaggia = new WindowSpiaggia();
            windowSpiaggia.Closed += (s, args) => Instance.Show();
            windowSpiaggia.Show();
        }

        private void btnRistorante_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            WindowRistorante windowRistorante = new WindowRistorante();
            windowRistorante.Closed += (s, args) => Instance.Show();
            windowRistorante.Show();
        }

        private void btnBilanci_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            WindowBilanci windowBilanci = new WindowBilanci();
            windowBilanci.Closed += (s, args) => Instance.Show();
            windowBilanci.Show();
        }

        private void btnEventi_Click(object sender, RoutedEventArgs e)
        {
            Instance.Hide();
            AddEvento_Ospite addEvento_Ospite = new AddEvento_Ospite();
            addEvento_Ospite.Closed += (s, args) => Instance.Show();
            addEvento_Ospite.Show();
        }
    }
}
