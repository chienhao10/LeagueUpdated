using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;
using System.Collections.Generic;
using System;


namespace hi_im_gosu_Reborn
{
    class Condemn
    {
        public static void Run()
            {
            foreach (var enemy in HeroManager.Enemies.Where(x =>x.IsValidTarget(Vayne.E.Range) && !x.HasBuffOfType(BuffType.SpellShield) && !x.HasBuffOfType(BuffType.SpellImmunity) && Condemn.canBeCondemned(x)))

                Vayne.E.CastOnUnit(enemy);

        }
       

        public static long Check;
        public static List<Vector2> Points = new List<Vector2>();
        
        public static bool canBeCondemned(Obj_AI_Hero unit, Vector2 pos = new Vector2())
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || Check + 50 > Environment.TickCount || ObjectManager.Player.IsDashing()) return false;        
            var pred = pos.IsValid() ? new List<Vector3>() { pos.To3D() } : new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                          Vayne.E.GetPrediction(unit).CastPosition,
                          Vayne.E.GetPrediction(unit).UnitPosition
                        };

            var walls = 0;
            Points = new List<Vector2>();
            foreach (var position in pred)
            {
                for (var i = 0; i < Vayne.emenu.Item("PushDistance").GetValue<Slider>().Value; i += (int)unit.BoundingRadius)
                {
                    var Pos = ObjectManager.Player.Position.Extend(position, ObjectManager.Player.Distance(position) + i).To2D();
                    Points.Add(Pos);
                    if (NavMesh.GetCollisionFlags(Pos.To3D()).
                        HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(Pos.To3D()).HasFlag(CollisionFlags.Building))
                    {
                        walls++;
                        break;
                    }
                }
            }
            if ((walls / pred.Count) >= 33 / 100f)
            {
                return true;
            }
            return false;
        }

    }
}
