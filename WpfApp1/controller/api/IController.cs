using System;
using System.Collections.Generic;
using WpfApp1.model.impl;

namespace WpfApp1.controller.api
{
    internal interface IController
    {
        void AggiungiOmbrellone(int numeroRiga, int numeroColonna, double prezzoGiornaliero);

        void RimuoviOmbrellone(int numeroRiga, int numeroColonna);

        void PrenotaOmbrellone(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine, string codiceFiscalePrenotante);

        void DisdiciOmbrellone(DateTime dataInzio, DateTime dataFine, int numeroRiga, int numeroColonna);

        bool ControlloOmbrelloneLibero(int numeroRiga, int numeroColonna, DateTime dataInzio, DateTime dataFine);

        int GetNumeroOmbrelloni();

        List<string> GetPrenotazioniOmbrellone(int numeroRiga, int numeroColonna); // ogni elemento della lista è un toString contenente informazioni relative a 1 prenotazione fatta sull'ombrellone

        double GetPrezzoGiornalieroOmbrellone(int numeroRiga, int numeroColonna);

        void AggiungiTavolo(int idTavolo, int numeroPosti);

        void RimuoviTavolo(int idTavolo);

        void PrenotaTavolo(int idTavolo);

        void DisdiciTavolo(int idTavolo, DateTime data, string pasto);

        int GetNumeroTavoli();

        void AggiungiCliente(string nome, string cognome, string numeroTelefono, int numeroPersoneOspiti, string città, string via, int civico, string email, string codiceDocumento, string codiceFiscale);

        // bool ControlloTavoloLibero();

    }
}
