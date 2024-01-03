using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_PROG.Utils
{
    class TextUtils
    {
        public TextUtils()
        {

        }

        public static void clearConsoleLine(int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth)); // Efface la ligne
            }
        }

        public static string colorText(string text)
        {
            return $"\x1b[91m{text}\x1b[39m"; // Red Color Text
        }
    }

}
