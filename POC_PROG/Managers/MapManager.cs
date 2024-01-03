using POC_PROG.Model;
using POC_PROG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POC_PROG.Managers
{
    class MapManager
    {
        public MapManager()
        {

        }

        /*=====================================================
                              LEVEL INIT
         ====================================================*/

        private static (int, int)[,] levelInfo = // Monster count, Chest count (peut être générer de façon aléatoire)
        {
            {(0, 0), (2, 0), (4, 0), (0, 0), (1, 1)},
            {(2, 2), (4, 1), (2, 0), (3, 0), (2, 0)},
            {(0, 0), (3, 0), (0, 2), (2, 1), (0, 2)},
            {(5, 2), (4, 1), (1, 1), (2, 0), (2, 0)},
            {(0, 4), (6, 0), (3, 2), (4, 0), (0, 0)}
        };

        private static (int, int)[] exit = { // peut être remplacer par une génération aléatoire
            (4, 4)
        };

        /*=====================================================
                            ROOMS INIT
         ====================================================*/

        public void createRooms()
        {
            for (int i = 0; i < levelInfo.GetLength(0); i++)
            {
                List<Room> row = new List<Room>();
                for (int j = 0; j < levelInfo.GetLength(1); j++)
                {
                    if (exit.Contains((i, j)))
                    {
                        row.Add(new Room(levelInfo[i, j].Item1, levelInfo[i, j].Item2, new List<int> { i, j }, true));
                    }
                    else
                    {
                        row.Add(new Room(levelInfo[i, j].Item1, levelInfo[i, j].Item2, new List<int> { i, j }));
                    }
                }
                level.Add(row);
            }
        }

        public void displayRoomInfo(int x, int y)
        {
            Console.WriteLine($"Vous entrez dans la salle. Position (x: {TextUtils.colorText(x.ToString())}, y: {TextUtils.colorText(y.ToString())})");
            Console.WriteLine($"Vous voyez : {TextUtils.colorText(level[x][y].getMonsterCount().ToString())} monstre(s).");
            Console.WriteLine($"Au fond de la salle il y a {TextUtils.colorText(level[x][y].getChestCount().ToString())} trésor(s).");
        }

        public static int[] getRoom(int x, int y)
        {
            int[] tab = { levelInfo[x, y].Item1, levelInfo[x, y].Item2 };
            return tab;
        }

        public static Room getRoomInstance(int x, int y)
        {
            return level[x][y];
        }

        public static void killMonster(int x, int y)
        {
            level[x][y].killMonster();
        }

        public static void openChest(int x, int y)
        {
            level[x][y].openChest();
        }

        public static int[] getLevelSize()
        {
            int[] tab = { levelInfo.GetLength(0), levelInfo.GetLength(1) };
            return tab;
        }

        private static List<List<Room>> level = new List<List<Room>>();
    }
}
