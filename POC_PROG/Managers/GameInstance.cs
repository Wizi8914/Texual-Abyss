using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using POC_PROG.Utils;

namespace POC_PROG.Managers
{
    class GameInstance
    {
        public GameInstance()
        {
            
        }

        public void Start()
        {
            // Initialisation des variables

            bool win = false; // Victoire
            int life = 15; // Vie du joueur
            MapManager mapManager = new MapManager();
            mapManager.createRooms();

            Console.WriteLine($"Bienvenue sur {TextUtils.colorText("Textual Abyss")}, veuillez entrée votre {TextUtils.colorText("pseudonyme")}");
            Console.WriteLine("\nPseudo : ");

            string name = Console.ReadLine();

            player = new Player(name, new Stats(life, 0), (0, 0));

            Console.Clear();
            Console.WriteLine("Bienvenue " + TextUtils.colorText(player.getName()) + " !");
            Console.WriteLine($"Vous commencer votre aventure avec {TextUtils.colorText(Convert.ToString(player.getLife()))} de vitalité ! Cependant votre score est à {TextUtils.colorText(Convert.ToString(player.getScore()))} ...");

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            ConsoleKeyInfo key = Console.ReadKey(false);

            Console.Clear();

            while (!win)
            {
                mapManager.displayRoomInfo(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2);
                Console.WriteLine("\n\nQue faites-vous parmis les choix suivants :");

                //Verification de l'input utilisateur

                List<string> choices = new List<string> { "haut", "bas", "gauche", "droite", "fouiller", "attaquer" };

                bool valid = false;
                string input = "";

                while (!valid)
                {
                    Console.WriteLine("1. Se déplacer (haut, bas, gauche, droite) ?");
                    Console.WriteLine("2. Attaquer ?");
                    Console.WriteLine("3. Fouiller ?");

                    Console.WriteLine("\nQue faites-vous parmis les choix suivants :");
                    input = Console.ReadLine().ToLower().Trim(); // On récupère l'input utilisateur et on le normalise (minuscule, plus on enlève les espaces)

                    if (choices.Contains(input))
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Veuillez entrer un choix {TextUtils.colorText("valide")} !\n");
                        mapManager.displayRoomInfo(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2);
                        Console.WriteLine("\n\nQue faites-vous parmis les choix suivants :");
                    }
                }

                switch (input)
                {
                    case "haut":
                        player.move("haut");
                        break;
                    case "bas":
                        player.move("bas");
                        break;
                    case "gauche":
                        player.move("gauche");
                        break;
                    case "droite":
                        player.move("droite");
                        break;
                    case "fouiller":
                        
                        break;
                    case "attaquer":
                        player.attack();
                        break;
                }

                Console.WriteLine("dégats");
                
                //player.damage(5);

            }
        }

        public static void Loose()
        {
            bool valid = false;
            string input = "";

            while (!valid)
            {
                Console.Clear();
                Console.WriteLine(@"
   _____          __  __ ______    ______      ________ _____    _ 
  / ____|   /\   |  \/  |  ____|  / __ \ \    / /  ____|  __ \  | |
 | |  __   /  \  | \  / | |__    | |  | \ \  / /| |__  | |__) | | |
 | | |_ | / /\ \ | |\/| |  __|   | |  | |\ \/ / |  __| |  _  /  | |
 | |__| |/ ____ \| |  | | |____  | |__| | \  /  | |____| | \ \  |_|
  \_____/_/    \_\_|  |_|______|  \____/   \/   |______|_|  \_\ (_)

===================================================================");

                Console.WriteLine($"\nVotre aventure s'arrête ici {TextUtils.colorText(player.getName())} , vous avez perdu !");
                Console.WriteLine($"Votre score final est de {TextUtils.colorText(Convert.ToString(player.getScore()))} !");

                Console.WriteLine("Que voulez vous faire ?");
                Console.WriteLine("1. Recommencer");
                Console.WriteLine("2. Quitter");

                Console.WriteLine("\nQue faites-vous parmis les choix suivants :");

                input = Console.ReadLine().ToLower().Trim();
                
                string[] choices = { "recommencer", "quitter" };

                if (choices.Contains(input))
                {
                    valid = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Veuillez entrer un choix {TextUtils.colorText("valide")} !\n");


                }
            }

            switch (input)
            {
                case "recommencer":

                    Console.WriteLine("\nAppuyez sur une touche pour recommencer...");
                    ConsoleKeyInfo key = Console.ReadKey(false);
                    Console.Clear();

                    GameInstance gameInstance = new GameInstance();
                    gameInstance.Start();

                    break;
                case "quitter":
                    Console.WriteLine("\nAppuyez sur une touche pour quitter...");
                    key = Console.ReadKey(false);
                    Environment.Exit(0);

                    break;
            }
        }

        static Player player;
    }
}
