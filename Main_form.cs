using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Text;
using System.Numerics;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace c_sharp_orbite
{
    public partial class Main_form : System.Windows.Forms.Form

    {

        //bool moveRight, moveLeft, moveUp,moveDown;
        private double vitesse;
        private double angle;
        private int Tempsecoule = 0;
        public double speed { get { return vitesse; } set { vitesse = value; } }
        public double angle_direction { get { return angle; } set { angle = value; } }
        static Random rand = new Random(); // initie une variable random pur les exemples
        static Random rand1 = new Random();
        static Random rand2 = new Random();
        static Position Position_vaisseau = new Position(rand.Next(1, 500), rand1.Next(1, 500)); // position du vaisseau
        static Position Position_planete = new Position(rand2.Next(1, 500), rand.Next(1, 500));// position de la planeète
        static Objet MonObjet_vaisseau = new Objet(100, Position_vaisseau.getter_x, Position_vaisseau.getter_y); // constructeur de objet pr le vaisseau
        static Objet MonObjet_planete = new Objet(rand2.Next(1, 400), Position_planete.getter_x, Position_planete.getter_y); //  constructeur de objet pr la planete
        static Planete planete = new Planete(500, MonObjet_planete.getter_poid, MonObjet_planete.getter_position_x, MonObjet_planete.getter_position_y); // constructeur de planete pr la planete

        public Main_form()
        {
            InitializeComponent(); // on initialize les composants du form

        }
        private void SetX_Y() // debut du jeux on initialise tout
        {
            vitesse = 100;
            var possition_planete_var = Position_planete.AfficheagePosition(); // position rendu en string de planete
            string poid_planete = "poids = " + planete.getter_poid.ToString(); // poid de la planete
            string diametre_planete = "diametre = " + planete.Diametre.ToString();// diametere planete
            string X = possition_planete_var.Item1; // x de planete
            string Y = possition_planete_var.Item2;// y de planete
            var possition_vaisseau_var = Position_vaisseau.AfficheagePosition(); // position rendu en string du vaisseau
            string poid_vaisseau = "poids = " + MonObjet_vaisseau.getter_poid.ToString(); // poid du vaisseau
            string X_vaisseau = possition_vaisseau_var.Item1; // x du vaisseau
            string Y_vaisseau = possition_vaisseau_var.Item2; // y du vaisseau
            Label1.Text = X;
            Label2.Text = Y;
            label3.Text = poid_planete;
            label4.Text = diametre_planete;
            label6.Text = X_vaisseau;
            label7.Text = Y_vaisseau;
            label8.Text = poid_vaisseau;
            var result = GetDistance(Convert.ToDouble(planete.getter_position_x), Convert.ToDouble(planete.getter_position_y), Convert.ToDouble(MonObjet_vaisseau.getter_position_x), Convert.ToDouble(MonObjet_vaisseau.getter_position_y)); // distance entre objet et planete
            label9.Text = "distance planete - vaisseau = " + result.ToString();
            label12.Text = "vitesse = " + vitesse.ToString();
            Vaisseau.Location = new Point(Position_vaisseau.getter_x, Position_vaisseau.getter_y); // initie la position du vaisseau
            Planete_game.Location = new Point(Position_planete.getter_x, Position_planete.getter_y); // initie la position de la planete
        } 
        private double AngleBeetweenObject()
        {
            float diffX = Planete_game.Left - Vaisseau.Left;
            float diffY = Planete_game.Top - Vaisseau.Top;

            double angle = Math.Atan2(diffY, diffX) * 180 / Math.PI;
            label10.Text = angle.ToString();
            return angle;
        }
        private void Begin(object sender, EventArgs e) // Start simulation
        {
            timer_game.Enabled = true; // initier le timer
            SetX_Y(); // fonction du debut de jeux
        }

        private static double GetDistance(double x1, double y1, double x2, double y2) // fonction pour calcuer la distance
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        private void Main_form_Load(object sender, EventArgs e) // load les objets du form
        {
            
        }

        private void exit(object sender, EventArgs e) // quitter le jeux
        {
            Close();
        }

        private void Crash() // crash du vaisseau
        {
            // effet de crash
            var original = Vaisseau.Location;
            var rnd = new Random(1454);
            const int shake_amplitude = 1000;
            for (int i = 0; i <= 9; i++)
            {
                Location = new Point(original.X + rnd.Next(-shake_amplitude, shake_amplitude), original.Y + rnd.Next(-shake_amplitude, shake_amplitude));
            }
            Location = original;
            GameOver_Button_Crash();
        }
        private void MoveTimerEvent(object sender, EventArgs e) // Boucle du jeux basé sur le timer
        {
            MoovePicture(sender, e); // boucle de jeux
        }

        private void MoovePicture(object sender, EventArgs e) // fonction utilisé pour update les info quand le vaisseau se deplace apres le debut du jeux
        {
            // boucle :
            angle_direction = 250; // angle de lancer souhaité
          
            var vecteur_direction =calculVecteur(Position_vaisseau.getter_x, Position_vaisseau.getter_y,angle_direction,vitesse); // calcul du vecteur
            double vecteur_direction_x = vecteur_direction.Item1; // on prend le x
            double vecteur_direction_y = vecteur_direction.Item2;// et le y du vecteur

            Vector2 vaisseau = new Vector2(((float)vecteur_direction_x), (float)vecteur_direction_y); // vecteur direction initial
            Vector2 earth = new Vector2(Position_planete.getter_x,Position_planete.getter_y); // planete
            Vector2 direction = earth + vaisseau; // vrai vecteur direction

            int start_x = Vaisseau.Left; // postion x du vaisseau au debut
            int start_y = Vaisseau.Top;// postion y du vaisseau au debut

            int end_x = (int)vecteur_direction_x;// postion x du vaisseau a la fin
            int end_y = (int)vecteur_direction_y;//postion y du vaisseau a la fin

            int duration = 10000; // durée du voyage
            Tempsecoule += timer_game.Interval; // on ajoute le temp écoulé au timer du jeux
            if (Tempsecoule >= duration) // ci le temps écoulé est supérieur a 10 secondes le jeux s'arrete
            {
                timer_game.Stop(); // stop le timer 
                GameOver_Button();// on affiche le button de fin
            }
            double progression = (double)Tempsecoule / duration; // progression
            int x = (int)(start_x + (end_x - start_x) * progression); // voyage x du vaisseau
            int y = (int)(start_y + (end_y - start_y) * progression); // voyage y du vaisseau

            var result = GetDistance(Convert.ToDouble(planete.getter_position_x), Convert.ToDouble(planete.getter_position_y), Convert.ToDouble(x), Convert.ToDouble(y));// distance planete vaisseau
            double gravité = ForceGravitationelle(MonObjet_vaisseau.getter_poid, planete.getter_poid, vitesse);// force gravitationelle
            double Angle_vaisseau_planete = AngleBeetweenObject(); // angle entre le vaisseau et la planete
            label10.Text = "angle planete vaisseau " + Angle_vaisseau_planete.ToString() + "degrés";
            label13.Text = "Force gravitationelle = " + gravité.ToString();
            label9.Text = "distance planete - vaisseau = " + result.ToString();
            label6.Text = "X = " + x.ToString() + "Km";
            label7.Text = "Y = " + y.ToString() + "Km";
            label11.Text = "angle de lancer = " + angle_direction.ToString() + "degres";
            Vaisseau.Left = x; // on change la position du vaisseau (x)
            Vaisseau.Top = y;// on change la position du vaisseau (y)
            // le vaisseau se deplacra vers x et y toutes les deux secondes pendant 10 secondes
            // le time interval est fixé a 2000 soit 2 secondes
            if (Position_vaisseau.getter_x == planete.getter_position_x || Position_vaisseau.getter_y == planete.getter_position_y) // ci le vaisseau touche la planete 
            {
                Crash(); // Crash le jeux s'arrete
            }
        }
        private void GameOver_Button() // fin du jeux
        {
            Button simulation_button = new Button();
            simulation_button.Text = "Le vaisseau est arrivé";
            simulation_button.Name = "simulation";
            simulation_button.Size = new Size(100, 200);
            simulation_button.Location = new Point(310, 3);
            simulation_button.BackColor = System.Drawing.Color.Red;
            simulation_button.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Controls.Add(simulation_button);

        }
        private void GameOver_Button_Crash() // fin du jeux
        {
            Button simulation_button = new Button();
            simulation_button.Text = "Le vaisseau c'est Crash";
            simulation_button.Name = "simulation";
            simulation_button.Size = new Size(100, 200);
            simulation_button.Location = new Point(310, 3);
            simulation_button.BackColor = System.Drawing.Color.Red;
            simulation_button.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Controls.Add(simulation_button);

        }
        private double ForceGravitationelle(int masse1,int masse2,double distance) // calculer la force gravitationelle
        {
            double G = 6.67 * Math.Pow(10, -11);
            double force = G * (masse1 * masse2) / Math.Pow(distance, 2);
            return force;
        }
        public ValueTuple<double,double> calculVecteur(int x,int y, double angle, double vitesse)// calculer le vecteur
        {
            double rad = angle * (Math.PI / 180);
            double newX = (x+ vitesse * Math.Cos(rad));
            double newY = (y + vitesse * Math.Sin(rad));
            return (newX, newY);
        }
    }
}