using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Lord_s_Vayne.Events
{
    class Play
    {
        public static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.Animation)
                {
                    case "Spell1a":
                        Game.SendEmote(Emote.Dance);
                        break;
                }
            }
        }
    }
}
