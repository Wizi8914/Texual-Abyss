using System;
using System.Collections.Generic;
using POC_PROG.Managers;

namespace TextualAbyss
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"
  _______        _               _            _                   
 |__   __|      | |             | |     /\   | |                  
    | | _____  _| |_ _   _  __ _| |    /  \  | |__  _   _ ___ ___ 
    | |/ _ \ \/ / __| | | |/ _` | |   / /\ \ | '_ \| | | / __/ __|
    | |  __/>  <| |_| |_| | (_| | |  / ____ \| |_) | |_| \__ \__ \
    |_|\___/_/\_\\__|\__,_|\__,_|_| /_/    \_\_.__/ \__, |___/___/
                                                     __/ |        
                                                    |___/         
===================================================================");

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey(false);

            Console.Clear();
            GameInstance instance = new GameInstance();

            instance.Start();
        }
    }
}