using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

            Console.WriteLine($"Bienvenue sur {TextUtils.colorText("Textual Abyss")}, veuillez entrer votre {TextUtils.colorText("pseudonyme")}");
            Console.WriteLine("\nPseudo : ");

            string name = Console.ReadLine();

            player = new Player(name, new Stats(life, 0), (0, 0));

            Console.Clear();
            Console.WriteLine("Bienvenue " + TextUtils.colorText(player.getName()) + " !");
            Console.WriteLine($"Vous commencez votre aventure avec {TextUtils.colorText(Convert.ToString(player.getLife()))} de vitalité ! Cependant votre score est à {TextUtils.colorText(Convert.ToString(player.getScore()))} ...");

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey(false);     

            Console.Clear();

            mapManager.displayRoomInfo(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2);


            // Boucle principale du jeu
            while (!win)
            {
                if (nextRoom) // Envoi le message si le joueur change de salle
                {
                    mapManager.displayRoomInfo(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2);
                }

                nextRoom = false;

                //Verification de l'input utilisateur

                List<string> choices = new List<string> { "haut", "bas", "gauche", "droite", "fouiller", "attaquer" };

                switch (getChoice(choices))
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
                        if (MapManager.getRoom(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2)[1] == 0 && !MapManager.getRoomInstance(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2).hasExit())
                        {
                            setErrorMessage($"\nVous avez fouillé dans le vide... Il n'y a pas de coffre {TextUtils.colorText("coffre")} ici !\n");
                            break;
                        }
                        if (MapManager.getRoomInstance(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2).getMonsterCount() > 0)
                        {
                            setErrorMessage($"\nIl y a encore {TextUtils.colorText(MapManager.getRoomInstance(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2).getMonsterCount().ToString())} dans la salle ! Exterminez les avant de pouvoir ouvrir les coffres.\n");
                            break;
                        }

                        if (MapManager.getRoom(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2)[1] > 0) // Lancer la condition de victoire lors de la fouille même si la salle est vide
                        {
                            player.openChest();
                        }

                        // Win condition

                        if (MapManager.getRoomInstance(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2).hasExit() &&
                            MapManager.getRoomInstance(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2).getChestCount() == 0 &&
                            MapManager.getRoomInstance(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2).getMonsterCount() == 0)
                        {
                            bool valid = false;
                            string input = "";
                            string exitErrorMessage = "";
                            bool alreadyExitErrorMessage = false;
                            bool exitError = false;

                            Console.WriteLine($"\nVous avez trouvé la {TextUtils.colorText("sortie")} ! Voulez-vous l'emprunter ? (Oui/Non)");
                            choices = new List<string> { "sortir", "explorer" };


                            while (!valid)
                            {
                                if (exitError)
                                {
                                    if (alreadyExitErrorMessage)
                                    {
                                        TextUtils.clearConsoleLine(11); // Clear les derniers messages + le message d'erreur pour en afficher un nouveau
                                    }
                                    else
                                    {
                                        TextUtils.clearConsoleLine(8);
                                        alreadyExitErrorMessage = true;
                                    }
                                }

                                Console.WriteLine("\n\nQue faites-vous parmis les choix suivants :");

                                if (exitError)
                                {
                                    Console.WriteLine(exitErrorMessage);
                                }

                                Console.WriteLine("1. Sortir ?");
                                Console.WriteLine("2. Explorer ?");

                                Console.WriteLine("\nChoix :");
                                input = Console.ReadLine().ToLower().Trim();

                                if (choices.Contains(input))
                                {
                                    valid = true;
                                }
                                else
                                {
                                    exitErrorMessage = $"\nVeuillez entrer un choix {TextUtils.colorText("valide")} !\n";
                                    exitError = true;
                                }
                            }

                            switch (input)
                            {
                                case "sortir":
                                    win = true;
                                    break;
                                case "explorer":
                                    break;
                            }
                        }

                        break;
                    case "attaquer":

                        if (MapManager.getRoomInstance(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2).getMonsterCount() == 0 && MapManager.getRoom(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2)[0] > 0)
                        {
                            setErrorMessage($"\nDu calme termiator ! Vous avez déjà {TextUtils.colorText("vaincu")} tout les monstres présent .\n");
                            break;
                        }

                        if (MapManager.getRoom(player.getCurrentCoords().Item1, player.getCurrentCoords().Item2)[0] == 0)
                        {
                            setErrorMessage($"\nVous avez attaqué dans le vide... Il n'y pas de monstre {TextUtils.colorText("monstre")} ici !\n");
                            break;
                        }
                        player.attack();
                        break;
                }
            }
            Win(); // Affiche l'écran de victoire
        }

        public static string getChoice(List<string> choices)
        {
            string input = "";
            bool valid = false;

            while (!valid)
            {
                if (error)
                {
                    if (alreadyErrorMessage)
                    {
                        TextUtils.clearConsoleLine(12); // Clear les derniers messages + le message d'erreur pour en afficher un nouveau
                    }
                    else
                    {
                        TextUtils.clearConsoleLine(9);
                        alreadyErrorMessage = true;
                    }
                }

                Console.WriteLine("\n\nQue faites-vous parmis les choix suivants :");

                if (error)
                {
                    Console.WriteLine(errorMessage);
                }

                Console.WriteLine("1. Se déplacer (haut, bas, gauche, droite) ?");
                Console.WriteLine("2. Attaquer ?");
                Console.WriteLine("3. Fouiller ?");

                Console.WriteLine("\nChoix :");
                input = Console.ReadLine().ToLower().Trim(); // On récupère l'input utilisateur et on le normalise (minuscule, plus on enlève les espaces)

                if (choices.Contains(input))
                {
                    valid = true;
                }
                else
                {
                    setErrorMessage($"\nVeuillez entrer un choix {TextUtils.colorText("valide")} !\n");
                }
            }

            return input;
        }   

        private static void Win()
        {
            bool valid = false;
            bool error = false;
            string input = "";

            while (!valid)
            {
                Console.Clear();
                Console.WriteLine(@"
 __     ______  _    _  __          _______ _   _   _ 
 \ \   / / __ \| |  | | \ \        / /_   _| \ | | | |
  \ \_/ / |  | | |  | |  \ \  /\  / /  | | |  \| | | |
   \   /| |  | | |  | |   \ \/  \/ /   | | | . ` | | |
    | | | |__| | |__| |    \  /\  /   _| |_| |\  | |_|
    |_|  \____/ \____/      \/  \/   |_____|_| \_| (_)
                                                      
======================================================");

                Console.WriteLine($"\nBravo {TextUtils.colorText(player.getName())}, vous vous êtes échappé des {TextUtils.colorText("Abyss")} vous allez pouvoir retrouver votre maman !");
                Console.WriteLine($"Votre score final est de {TextUtils.colorText(Convert.ToString(player.getScore()))} !");

                Console.WriteLine("\nQue voulez vous faire ?");

                if (error)
                {
                    Console.WriteLine($"\nVeuillez entrer un choix {TextUtils.colorText("valide")} !\n");
                }

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
                    error = true;
                }
            }

            switch (input)
            {
                case "recommencer":

                    Console.WriteLine("\nAppuyez sur une touche pour recommencer...");
                    Console.ReadKey(false);
                    Console.Clear();

                    GameInstance gameInstance = new GameInstance();
                    gameInstance.Start();

                    break;
                case "quitter":
                    Console.WriteLine("\nAppuyez sur une touche pour quitter...");
                    Console.ReadKey(false);
                    Environment.Exit(0);

                    break;
            }
        }

        public static void Loose()
        {
            bool valid = false;
            bool error = false;
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

                Console.WriteLine($"\nVotre aventure s'arrête ici {TextUtils.colorText(player.getName())}, vous avez perdu !");
                Console.WriteLine($"Votre score final est de {TextUtils.colorText(Convert.ToString(player.getScore()))} !");

                Console.WriteLine("\nQue voulez vous faire ?");

                if (error)
                {
                    Console.WriteLine($"\nVeuillez entrer un choix {TextUtils.colorText("valide")} !\n");
                }

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
                    error = true;
                }
            }

            switch (input)
            {
                case "recommencer":

                    Console.WriteLine("\nAppuyez sur une touche pour recommencer...");
                    Console.ReadKey(false);
                    Console.Clear();

                    GameInstance gameInstance = new GameInstance();
                    gameInstance.Start();

                    break;
                case "quitter":
                    Console.WriteLine("\nAppuyez sur une touche pour quitter...");
                    Console.ReadKey(false);
                    Environment.Exit(0);

                    break;
            }
        }

        // Getters & Setters

        public static void setErrorMessage(string message)
        {
            error = true;
            errorMessage = message;
        }

        public static void setError(bool err)
        {
            error = err;
            alreadyErrorMessage = err;
        }

        public static void setNextRoom(bool value)
        {
            nextRoom = value;
            error = false;
            alreadyErrorMessage = false;
        }

        static Player player;

        private static bool nextRoom = false;
        private static bool error = false;
        private static string errorMessage = "";
        private static bool alreadyErrorMessage = false;
    }
}
