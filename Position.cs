using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_orbite
{
    class Position
    {
        private int x;
        private int y;  //initie les deux variables
        public int getter_x { get { return x; } set { x = value; } } // getter des deux variables
        public int getter_y { get { return y; } set { y = value; } }

        public Position(int x, int y) // constructeur
        {
            this.x = x;
            this.y = y;
        }
        public ValueTuple<string ,string> AfficheagePosition()  // fonction qui return l'int en string avk le Km
        {
            string string_x = "X = " + x.ToString() + "Km"; //transformer l'int x en string
            string string_y = "Y = " + y.ToString() + "Km";//transformer l'int y en string
            return (string_x,string_y); // return les deux
        }



    }
}