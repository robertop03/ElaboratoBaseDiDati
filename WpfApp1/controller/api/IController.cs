using System;
using System.Collections.Generic;

namespace WpfApp1.controller.api
{
    internal interface IController
    {
        #region Ombrellone

        void AggiungiOmbrellone(int numeroRiga, int numeroColonna);

        void RimuoviOmbrellone(int numeroRiga, int numeroColonna);

        void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine, string codiceFiscalePrenotante, int numeroLettiniAggiuntivi);

        void DisdiciOmbrellone(DateTime dataInzio, DateTime dataFine, int numeroRiga, int numeroColonna);

        bool ControlloOmbrelloneLibero(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine);

        int GetNumeroOmbrelloni();

        List<string> GetPrenotazioniOmbrellone(int numeroRiga, int numeroColonna); // ogni elemento della lista è un toString contenente informazioni relative a 1 prenotazione fatta sull'ombrellone

        List<(int, int)> OmbrelloniPrenotati(DateTime data); // restituisce una lista con gli tutti gli ombrelloni che hanno una prenotazione in una determinata data.

        #endregion

        #region Tavolo

        void AggiungiTavolo(int idTavolo, int numeroPosti);

        void RimuoviTavolo(int idTavolo);

        void PrenotaTavolo(int idTavolo, DateTime data, string pasto, string codiceFiscalePrenotante, int numeroPersonePrenotanti);

        void DisdiciTavolo(int idTavolo, DateTime data, string pasto);

        bool ControlloTavoloLibero(int idTavolo, DateTime data, string pasto);

        int GetNumeroTavoli();

        List<string> GetPrenotazioniTavolo(int idTavolo);

        List<int> TavoliPrenotati(DateTime data);

        bool NumeroPostiTavoloAdegueato(int idTavolo, int numeroOspiti); // restituisce true se il numero di posti del tavolo è sufficente a contenere gli ospiti per il quale si sta prenotando.

        #endregion

        #region Menu, piatti e ordini

        void AggiungiPiatto(string nome, double prezzo, string descrizione);

        void RimuoviPiatto(string nome);

        void AggiungiMenu(int idMenu, double prezzo, List<string> piatti);

        void RimuoviMenu(int idMenu);

        List<string> GetPiatti(); // restituisce tutti i piatti attuali 

        List<string> GetMenu(); // restituisce tutti i menu attuali

        void AggiungiOrdine(int idOrdine, DateTime data, string pasto, int idTavolo, List<int> idMenuOrdinati, List<string> nomiPiattiOrdinati);

        #endregion

        void AggiungiCliente(string nome, string cognome, string numeroTelefono, string città, string via, int civico, string email, string codiceDocumento, string codiceFiscale);

        List<string> GetClienti();

        #region Eventi e Ospiti

        void AggiungiEvento(string titolo, DateTime data, TimeSpan orario, string descrizione, double costoIngrezzo, List<string> ospitiEvento);

        void RimuoviEvento(string titolo, DateTime data);

        List<string> GetEvents();

        void AggiungiOspite(string codiceFiscale, string cognome, string nome, string numeroTelefono, string nickname);

        void RimuoviOspite(string codiceFiscale);

        List<string> GetOspiti();

        #endregion

        List<string> GetEmails();
    }
}
