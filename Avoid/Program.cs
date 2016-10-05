using LeagueSharp;
using LeagueSharp.Common;

namespace Avoid
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            Utils.ClearConsole();
#endif
            CustomEvents.Game.OnGameLoad += uselessArgs => Avoid.OnGameStart();
            Game.PrintChat("<font size='30'>Avoid</font> <font color='#b756c5'>by LordZEDith</font>");
        }
    }
}
