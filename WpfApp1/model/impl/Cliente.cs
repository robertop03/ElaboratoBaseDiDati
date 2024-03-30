using System;
using System.Text.RegularExpressions;

namespace WpfApp1.model.impl
{
    internal class Cliente : Persona
    {
        private int numeroPersoneOspiti;
        private string città;
        private string via;
        private int civico;
        private string email;
        private readonly string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        public int NumeroPersoneOspiti
        {
            get => numeroPersoneOspiti;
            set => numeroPersoneOspiti = value >= 1 && value <= 8
                    ? value
                    : throw new ArgumentOutOfRangeException("Il numero di persone ospiti deve essere compreso tra 1 e 8.");
        }

        public string Città
        {
            get => città;
            set => città = !string.IsNullOrEmpty(value)
                    ? value
                    : throw new ArgumentException("La città non può essere vuota.");
        }

        public string Via
        {
            get => via;
            set => via = !string.IsNullOrEmpty(value)
                    ? value
                    : throw new ArgumentException("La via non può essere vuota o null.");
        }

        public int Civico
        {
            get => civico;
            set => civico = value >= 1 && value <= 1000
                    ? value
                    : throw new ArgumentOutOfRangeException("Il civico deve essere compreso tra 1 e 1000.");
        }

        public string Email
        {
            get => email;
            set => email = Regex.IsMatch(email, pattern)
                    ? value
                    : throw new ArgumentException("Formato della mail non valido.");
        }

        public Documento Documento { get; set; } = null;

        public Cliente(int numeroPersoneOspiti, string città, string via, int civico, string email, string codiceDocumento, string codiceFiscale, string nome, string cognome, string numeroTelefono)
            : base(codiceFiscale, nome, cognome, numeroTelefono)
        {
            NumeroPersoneOspiti = numeroPersoneOspiti;
            Città = città;
            Via = via;
            Civico = civico;
            Email = email;
        }
    }
}
