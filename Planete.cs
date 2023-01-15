using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_orbite
{
    class Planete : Objet // herite de la calsse objet
    {
        private int diametre; // diametre
        public int Diametre { get { return diametre; } set { diametre = value; } } // getter diametre

        public Planete(int diametre,int poid,int x, int y):base(poid,x,y) // constructeur
            
        {
            this.diametre = diametre;
        }
    }
}
