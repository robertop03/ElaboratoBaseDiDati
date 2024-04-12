namespace WpfApp1.model.impl
{
    internal class Ospite : Persona
    {
        public string Nickname { get; set; }

        public Ospite(string codiceFiscale, string nome, string cognome, string numeroTelefono, string nickname) : base(codiceFiscale, nome, cognome, numeroTelefono)
        {
            Nickname = nickname;
        }

        public override string ToString()
        {
            return $"Nome: {Nome}, Cognome: {Cognome}, CF: {CodiceFiscale}, Numero: {NumeroTelefono} Nickname: {Nickname}";
        }
    }
}
