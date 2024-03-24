using System.ComponentModel;

namespace WpfApp1.model.impl
{
    internal abstract class AbstractOggettoPrenotabile
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private bool occupato;
        public bool Occupato
        {
            get { return occupato; }
            set
            {
                if(occupato != value)
                {
                    occupato = value;
                    OnPropertyChanged(nameof(Occupato));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}