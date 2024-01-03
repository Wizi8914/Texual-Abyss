using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_PROG.Model
{
    class Room
    {
        public Room(int monsterCount, int chestCount, List<int> coords, bool hasExit = false)
        {
            _monsterCount = monsterCount;
            _chestCount = chestCount;
            _coords = coords;
            _hasExit = hasExit;
        }

        public int getMonsterCount()
        {
            return _monsterCount;
        }

        public int getChestCount()
        {
            return _chestCount;
        }

        public List<int> getCoords()
        {
            return _coords;
        }

        public void openChest()
        {
            _chestCount--;
        }

        public void killMonster()
        {
            _monsterCount--;
        }

        public bool hasExit()
        {
            return _hasExit;
        }

        private int _monsterCount;
        private int _chestCount;
        private List<int> _coords;
        private bool _hasExit;
    }
}
