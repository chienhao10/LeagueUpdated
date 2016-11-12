using LeagueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectWardReborn
{
    class Program
    {
      public static Helper Helper;

        static void Main(string[] args)
        {

            new PerfectWardTracker();
            Game.PrintChat("<font size='30'>PerfectWard Reborn</font> <font color='#b756c5'>by LordZEDith</font>");
        }
    }
}
