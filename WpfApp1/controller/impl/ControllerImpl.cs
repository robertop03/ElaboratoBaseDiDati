using System;
using System.Collections.ObjectModel;
using WpfApp1.controller.api;
using WpfApp1.model.impl;

namespace WpfApp1.controller.impl
{
    internal class ControllerImpl : IController
    {
        public ObservableCollection<Ombrellone> ListaOmbrelloni { get; set; }
        public ObservableCollection<Tavolo> ListaTavoli { get; set; }

        public event EventHandler AggiuntoOmbrellone;
        public event EventHandler AggiuntoTavolo;

        public ControllerImpl()
        {
            ListaOmbrelloni = new ObservableCollection<Ombrellone>();
            ListaTavoli = new ObservableCollection<Tavolo>();
        }

        public void AggiungiOmbrellone(int numeroRiga, int numeroColonna)
        {
            Ombrellone ombrellone = new Ombrellone(numeroRiga, numeroColonna);
            ListaOmbrelloni.Add(ombrellone);
            AggiuntoOmbrellone?.Invoke(this, EventArgs.Empty);
        }

        public void AggiungiTavolo()
        {
            Tavolo tavolo = new Tavolo();
            ListaTavoli.Add(tavolo);
            AggiuntoTavolo?.Invoke(this, EventArgs.Empty);
        }

        public void DisdiciOmbrellone(Ombrellone ombrellone)
        {
            ombrellone.Occupato = false;
        }

        public void DisdiciTavolo(Tavolo tavolo)
        {
            tavolo.Occupato = false;
        }

        public void PrenotaOmbrellone(Ombrellone ombrellone)
        {
            ombrellone.Occupato = true;
        }

        public void PrenotaTavolo(Tavolo tavolo)
        {
            tavolo.Occupato = true;
        }

        public void RimuoviOmbrellone(Ombrellone ombrellone)
        {
            _ = ListaOmbrelloni.Remove(ombrellone);
        }

        public void RimuoviTavolo(Tavolo tavolo)
        {
            _ = ListaTavoli.Remove(tavolo);
        }

        public int GetNumeroTavoli()
        {
            return ListaTavoli.Count;
        }

        public int GetNumeroOmbrelloni()
        {
            return ListaOmbrelloni.Count;
        }

        public string InfoTavolo()
        {
            throw new NotImplementedException();
        }

        public string InfoOmbrellone()
        {
            throw new NotImplementedException();
        }
    }
}
