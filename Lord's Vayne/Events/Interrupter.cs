using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Lord_s_Vayne.Events
{
    class Interrupter
    {
        public static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero unit, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (Program.imenu.Item("Int_E", true).GetValue<bool>() && Program.E.IsReady() && unit.IsEnemy &&
                unit.IsValidTarget(Program.E.Range))
            {
                if (args.DangerLevel >= Interrupter2.DangerLevel.High)
                {
                    Program.E.CastOnUnit(unit, true);
                }
            }
        }
    }
}
