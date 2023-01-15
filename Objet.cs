using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_orbite
{
    class Objet
    {
        private int position_x; // X
        private int position_y; // Y
        private int poid; // poid
        public int getter_poid { get { return poid; } set { poid = value; } } // getter poid
        public int getter_position_x { get { return position_x; } set { position_x = value; } } // getter x
        public int getter_position_y { get { return position_y; } set { position_y = value; } } // getter y

        public Objet(int poid,int x,int y) // constructeur
        {
            this.poid = poid;
            this.position_x = x;
            this.position_y = y;
        }

    }
}
