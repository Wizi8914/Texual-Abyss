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

        public static string colorText(string text)
        {

            return $"\x1b[91m{text}\x1b[39m"; // Red Color Text
        }
    }

}
