using System;
using System.Collections.Generic;

namespace WpfApp1.controller.api
{
    internal interface IController
    {
        void AggiungiOmbrellone(int numeroRiga, int numeroColonna);

        void RimuoviOmbrellone(int numeroRiga, int numeroColonna);

        void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine, string codiceFiscalePrenotante);

        void DisdiciOmbrellone(DateTime dataInzio, DateTime dataFine, int numeroRiga, int numeroColonna);

        bool ControlloOmbrelloneLibero(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine);

        int GetNumeroOmbrelloni();

        List<string> GetPrenotazioniOmbrellone(int numeroRiga, int numeroColonna); // ogni elemento della lista è un toString contenente informazioni relative a 1 prenotazione fatta sull'ombrellone

        void AggiungiTavolo(int idTavolo, int numeroPosti);

        void RimuoviTavolo(int idTavolo);

        void PrenotaTavolo(int idTavolo, DateTime data, string pasto, string codiceFiscalePrenotante);

        void DisdiciTavolo(int idTavolo, DateTime data, string pasto);

        bool ControlloTavoloLibero(int idTavolo, DateTime data, string pasto);

        int GetNumeroTavoli();

        List<string> GetPrenotazioniTavolo(int idTavolo);

        bool NumeroPostiTavoloAdegueato(int idTavolo, int numeroOspiti); // restituisce true se il numero di posti del tavolo è sufficente a contenere gli ospiti per il quale si sta prenotando.

        void AggiungiCliente(string nome, string cognome, string numeroTelefono, int numeroPersoneOspiti, string città, string via, int civico, string email, string codiceDocumento, string codiceFiscale);

        // bool ControlloTavoloLibero();

    }
}
