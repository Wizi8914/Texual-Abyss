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
                ConsoleKeyInfo key = Console.ReadKey(false);
                Console.Clear();

                GameInstance.setNextRoom(true);
                clearRoom = false;
            }
        }

        public void attack()
        {
            if (clearRoom)
            {
                GameInstance.setErrorMessage($"\nDu calme termiator ! Vous avez déjà {TextUtils.colorText("vaincu")} tout les monstres présent .\n");
                return;
            }

            Random rdm = new Random();
            int damage = (int)Math.Ceiling((double)MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getMonsterCount() / 2);

            playerDamage(damage);

            List<string> attackMessages = new List<string>
            {
                $"Vous attaquez ! Le monstre a été désintégré par votre sainte puissance ! [Mais ses camarades se vengent...] vous perdez {TextUtils.colorText(damage.ToString())} points de vie ! Il vous reste {TextUtils.colorText(_currentLife.ToString())} points de vie.",
                $"Vous avez prit la bonne décision ! Votre attaque a démolit le petit monstre sans défense ! [Ses potes arrivent !] vous perdez {TextUtils.colorText(damage.ToString())} point de vie ! Il vous reste {TextUtils.colorText(_currentLife.ToString())} points de vie."
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
                clearRoom = true;
            }
            else
            {
                MapManager.killMonster(_currentCoords.Item1, _currentCoords.Item2);
                Console.WriteLine($"\nIl reste encore {TextUtils.colorText(MapManager.getRoomInstance(_currentCoords.Item1, _currentCoords.Item2).getMonsterCount().ToString())} amis à lui ! Que compte tu faire ?");
                GameInstance.setError(false);
            }           
        }

        private bool clearRoom = false;


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
