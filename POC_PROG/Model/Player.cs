using POC_PROG.Utils;
using POC_PROG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace POC_PROG
{
    class Player
    {
        public Player(string name, Stats stats, (int, int) currentCoords)
        {
            _name = name;
            _stats = stats;
            _currentLife = stats.getLife();
            _currentCoords = currentCoords;
        }

        // ACTIONS

        public void move(string movement)
        {
            switch (movement)
            {
                case "haut":
                    if (_currentCoords.Item1 == 0)
                    {
                        GameInstance.setErrorMessage($"\nVous ne pouvez pas aller {TextUtils.colorText("plus haut")} !\n");
                        break;
                    }

                    _currentCoords.Item1--;
                    moveRoom($"\nVous vous déplacez vers le {TextUtils.colorText("haut")} !");
                    break;

                case "bas":
                    if (_currentCoords.Item1 == MapManager.getLevelSize()[0]-1)
                    {
                        GameInstance.setErrorMessage($"\nVous ne pouvez pas aller {TextUtils.colorText("plus bas")} !\n");
                        break;
                    }

                    _currentCoords.Item1++;
                    moveRoom($"\nVous vous déplacez vers le {TextUtils.colorText("bas")} !");
                    break;

                case "gauche":
                    if (_currentCoords.Item2 == 0)
                    {
                        GameInstance.setErrorMessage($"\nVous ne pouvez pas aller {TextUtils.colorText("plus à gauche")} !\n");
                        break;
                    }

                    _currentCoords.Item2--;
                    moveRoom($"\nVous vous déplacez vers la {TextUtils.colorText("gauche")} !");
                    break;

                case "droite":
                    if (_currentCoords.Item2 == MapManager.getLevelSize()[1]-1)
                    {
                        GameInstance.setErrorMessage($"\nVous ne pouvez pas aller {TextUtils.colorText("plus à droite")} !\n");
                        break;
                    }
                    _currentCoords.Item2++;
                    moveRoom($"\nVous vous déplacez vers la {TextUtils.colorText("droite")} !");
                    break;
            }

            void moveRoom(string message)
            {
                Console.WriteLine(message);
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                Console.Clear();

                GameInstance.setNextRoom(true);
                clearRoomMonster = false;
            }
        }

        public void attack()
        {
            if (clearRoomMonster)
            {
                GameInstance.setErrorMessage($"\nDu calme termiator ! Vous avez déjà {TextUtils.colorText("vaincu")} tout les monstres présents.\n");
                return;
            }

            Random rdm = new Random();
            int damage = (int)Math.Ceiling((double)MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getMonsterCount() / 2);

            playerDamage(damage);

            List<string> attackMessages = new List<string>
            {
                "Vous attaquez ! Le monstre a été désintégré par votre sainte puissance ! " + ((MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getMonsterCount() > 1) ? "Mais ses camarades se vengent... " : "") + $"Vous perdez {TextUtils.colorText(damage.ToString())} points de vie ! Il vous reste {TextUtils.colorText(_currentLife.ToString())} points de vie.",
                "Vous avez prit la bonne décision ! Votre attaque a démolit le petit monstre sans défense ! " + ((MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getMonsterCount() > 1) ? "Ses potes arrivent ! " : "") + $"Vous perdez {TextUtils.colorText(damage.ToString())} point de vie ! Il vous reste {TextUtils.colorText(_currentLife.ToString())} points de vie."
            };

            if (MapManager.getRoom(_currentCoords.Item1, _currentCoords.Item2)[0] == 1)
            {
                Console.WriteLine($"Vous attaquez ! Le monstre a été désintégré par votre sainte puissance ! Mais dans sa chute il vous a mordu... vous perdez {TextUtils.colorText(damage.ToString())} points de vie !");
            }

            Console.WriteLine("\n" + attackMessages[rdm.Next(attackMessages.Count())]);

            if (MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getMonsterCount()-1 == 0)
            {
                MapManager.killMonster(_currentCoords.Item1, _currentCoords.Item2); // On tue le dernier monstre
                Console.WriteLine($"\nVous avez {TextUtils.colorText("vaincu")} tous les monstres de la salle ! Vous pouvez continuer votre aventure !");
                GameInstance.setError(false);
                clearRoomMonster = true;
            }
            else
            {
                MapManager.killMonster(_currentCoords.Item1, _currentCoords.Item2);
                Console.WriteLine($"\nIl reste encore {TextUtils.colorText(MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getMonsterCount().ToString())} amis à lui ! Que compte tu faire ?");
                GameInstance.setError(false);
            }           
        }

        private bool clearRoomMonster = false;

        public void openChest()
        {
            if(clearRoomChest)
            {
                GameInstance.setErrorMessage($"\nPlus de {TextUtils.colorText("coffres")} à l'horizon. Vous les avez tous trouvés !\n");
                return;
            }

            Console.WriteLine("\nVous ouvrez le coffre...");
            
            Random rdm = new Random();
            int hp = rdm.Next(0, 6);

            string[] noChestText = {
                "Coffre vide. Apparemment, quelqu'un a confondu 'butin' avec 'chasse aux fantômes'.",
                "Vous ouvrez le coffre, mais il semble que le trésor ait pris des vacances. Rien à voir ici !",
                "Rien dans le coffre. Même les araignées l'ont abandonné, préférant les coins moins ennuyeux.",
                "Vous découvrez un coffre sans trésor, la honte des coffres",
                "Le coffre est vide, mais votre imagination est pleine de trésors. Lequel préférez-vous ?",
                "Aucun trésor ici, mais peut-être que le véritable trésor était l'amitié que vous avez développée avec le coffre.",
                "Vous espériez de l'or, mais ce coffre est tellement vide qu'il en devient économe.",
            };

            heal(hp); // On heal le joueur

            switch (hp)
            {
                case 0:
                    Console.WriteLine($"\n{noChestText[new Random().Next(noChestText.Length)]}");
                    break;
                case 1:
                    Console.WriteLine($"\nVous avez trouvé un vieux bout de pain ! Vous le mangez et regagnez {TextUtils.colorText("1")} point de vie ! Vous avez désormais {TextUtils.colorText(_currentLife.ToString())} points de vie.");
                    break;
                case 2:
                    Console.WriteLine($"\nVous avez trouvé une pomme ! Vous la mangez et regagnez {TextUtils.colorText("2")} points de vie ! Vous avez désormais {TextUtils.colorText(_currentLife.ToString())} points de vie.");
                    break;
                case 3:
                    Console.WriteLine($"\nVous découvrez une potion de soin ! En la buvant, vous regagnez {TextUtils.colorText("3")} points de vie ! Vous avez désormais {TextUtils.colorText(_currentLife.ToString())} points de vie.");
                    break;
                case 4:
                    Console.WriteLine($"\nVous mettez la main sur une potion de soin de qualité supérieure ! Sa consommation vous redonne {TextUtils.colorText("4")} points de vie ! Vous avez désormais {TextUtils.colorText(_currentLife.ToString())} points de vie.");
                    break;
                case 5:
                    Console.WriteLine($"\nVous trouvez une potion de soin puissante ! Elle vous restaure {TextUtils.colorText("5")} points de vie et revitalise votre énergie ! Vous avez désormais {TextUtils.colorText(_currentLife.ToString())} points de vie.");
                    break;
            }


            int score = 10;
            _stats.addScore(score);

            MapManager.openChest(_currentCoords.Item1, _currentCoords.Item2);

            Console.WriteLine($"\nVous avez gagné {TextUtils.colorText(score.ToString())} points, ils sont ajoutés à votre score ! Vous pourrez aller le raconter à votre maman une fois sortie d'ici ! Votre score est maintenant de {TextUtils.colorText(_stats.getScore().ToString())}.");
            
            if (MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getChestCount() == 0)
            {
                Console.WriteLine($"\nIl semblerait que vous ayez décidé de faire une tournée mondiale des coffres ! Félicitations, vous avez désormais un doctorat en ouverture de {TextUtils.colorText("coffres")}. Vous avez le choix de changer de salle. Oui, il y a d'autres salles. Incroyable, non ?");
                GameInstance.setError(false);
                clearRoomChest = true;
            }
            else
            {
                Console.WriteLine($"\nIl semblerais qu'il reste encore {TextUtils.colorText(MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getChestCount().ToString())} coffre a ouvrir !");
                GameInstance.setError(false);
            }
        }

        private bool clearRoomChest = false;

        // GETTERS

        public string getName()
        {
            return _name;
        }

        public int getLife()
        {
            return _currentLife;
        }

        public int getScore()
        {
            return _stats.getScore();
        }

        public (int, int) getCurrentCoords()
        {
            return _currentCoords;
        }

        // SETTERS
        public void playerDamage(int damage)
        {
            _currentLife -= damage;

            if (_currentLife <= 0)
            {
                GameInstance.Loose();
            }
        }

        public void heal(int heal)
        {
            _currentLife += heal;
        }

        private string _name;
        private int _currentLife;
        private Stats _stats;
        private (int, int) _currentCoords;

    }
}
