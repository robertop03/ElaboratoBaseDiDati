using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.model.impl
{
    internal class Menu
    {
        public int IdMenu { get; set; }
        public List<Piatto> ListaPiatti { get; set; }
        public double Prezzo { get; set; }

        public Menu(int idMenu, List<Piatto> listaPiatti, double prezzo)
        {
            IdMenu = idMenu;
            ListaPiatti = listaPiatti;
            Prezzo = prezzo;
        }

        public override string ToString()
        {
            string result = $"Menù n°: {IdMenu}\nPrezzo: {Prezzo}\n";

            foreach (Piatto piatto in ListaPiatti)
            {
                result += $"    {piatto}\n";
            }

            return result;
        }
    }
}
