using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeMaker.SudoLib
{
    public static class g
    {
        public const int PSIZE = 9;
        public static void Banner(string msg)
        {
            Console.WriteLine(new string('=', (int)(msg.Length * 1.5)));
            Console.Write(new string(' ', 10));
            Console.WriteLine(msg);
            Console.WriteLine(new string('=', (int)(msg.Length * 1.5)));
            Console.WriteLine();
        }

        public static void Pause()
        {
            Console.Write("Press any key to Continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

    }
}
