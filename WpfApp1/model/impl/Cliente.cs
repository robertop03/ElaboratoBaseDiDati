namespace WpfApp1.model.impl
{
    internal class Cliente : Persona
    {
        public string Città { get; set; }

        public string Via { get; set; }

        public int Civico { get; set; }

        public string Email { get; set; }

        public Documento Documento { get; set; } = null;

        public Cliente(string città, string via, int civico, string email, string codiceDocumento, string codiceFiscale, string nome, string cognome, string numeroTelefono)
            : base(codiceFiscale, nome, cognome, numeroTelefono)
        {
            Città = città;
            Via = via;
            Civico = civico;
            Email = email;
        }
    }
}
