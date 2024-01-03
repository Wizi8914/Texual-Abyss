using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_PROG
{
    class Stats
    {
        public Stats(int life, int score)
        {
            _life = life;
            _score = score;

        }

        public int getLife()
        {
            return _life;
        }

        public int getScore()
        {
            return _score;
        }

        public void addScore(int score)
        {
            _score += score;
        }

        private int _life;
        private int _score;
    }
}
