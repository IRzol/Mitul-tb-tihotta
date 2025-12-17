using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Versenyzok
{
    internal class Versenyzo
    {
        public Versenyzo(int id, string nev, double ido1, double ido2, double ido3)
        {
            Id = id;
            Nev = nev;
            Ido1 = ido1;
            Ido2 = ido2;
            Ido3 = ido3;
        }

        public Versenyzo() { }

        public int Id { get; private set; }
        public string Nev { get; set; }
        public double Ido1 { get; set; }
        public double Ido2 { get; set; }
        public double Ido3 { get; set; }

 
        private int SzamolPont(double ido)
        {
            int pont = 10 - (int)Math.Floor(ido);
            if (pont < 0) return 0;
            return pont;
        }
        public int Pontszam1
        {
            get { return SzamolPont(Ido1); }
        }

        public int Pontszam2
        {
            get { return SzamolPont(Ido2); }
        }

        public int Pontszam3
        {
            get { return SzamolPont(Ido3); }
        }

        public int LegjobbPont()
        {
            int legtobbPont = 0;
            if (Pontszam1 > Pontszam2 && Pontszam1 > Pontszam3)
            {
                legtobbPont = Pontszam1;
            }

            else if (Pontszam2 > Pontszam1 && Pontszam2 > Pontszam3)
            {
                legtobbPont = Pontszam2;
            }

            else if (Pontszam3 > Pontszam2 && Pontszam3 > Pontszam1)
            {
                legtobbPont = Pontszam3;
            }
            return legtobbPont;

        }

        public override string ToString()
        {
            return $"{Id}; {Nev}; {Pontszam1}; {Pontszam2}; {Pontszam3}; {LegjobbPont()}";
        }
    }
}