using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAwareness
{
    class Program
    {
        private static ExtendedAwareness extendedAwareness;

        static void Main(string[] args)
        {
            Game.PrintChat("<font size='30'>xAwarness</font> <font color='#b756c5'>by LordZedith</font>");
            extendedAwareness = new ExtendedAwareness();
        }
    }
}
