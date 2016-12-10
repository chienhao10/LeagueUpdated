using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Lord_s_Vayne.QLogic
{
    class Cursor
    {
        public static void Run()
        {
            Program.Q.Cast(Game.CursorPos);
        }
    }
}
