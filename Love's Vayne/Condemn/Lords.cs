﻿using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Lord_s_Vayne.Condemn
{
    class Lords
    {
        public static void RotE()
        {
            var target = TargetSelector.GetTarget(Program.zzrot.Range, TargetSelector.DamageType.Physical);
            if (Program.E.IsReady() && Program.zzrot.IsInRange(target) && Program.zzrot.IsReady() && target != null)
            {
                if (Program.zzrot.Cast(target.ServerPosition.To2D()))
                {
                    Program.E2.CastOnUnit(target);
                }
            }
        }
        public static void Run()
        {
            foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(Program.E.Range) && !x.HasBuffOfType(BuffType.SpellShield) && !x.HasBuffOfType(BuffType.SpellImmunity) && canBeCondemned(x)))

                Program.E.CastOnUnit(enemy);

        }


        public static long Check;
        public static List<Vector2> Points = new List<Vector2>();
        private static SpellSlot FlashSlot;

        public static bool canBeCondemned(Obj_AI_Hero unit, Vector2 pos = new Vector2())
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || Check + 50 > Environment.TickCount || ObjectManager.Player.IsDashing()) return false;
            var pred = pos.IsValid() ? new List<Vector3>() { pos.To3D() } : new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                          Program.E.GetPrediction(unit).CastPosition,
                          Program.E.GetPrediction(unit).UnitPosition
                        };

            var walls = 0;
            Points = new List<Vector2>();
            foreach (var position in pred)
            {
                for (var i = 0; i < Program.emenu.Item("PushDistance").GetValue<Slider>().Value; i += (int)unit.BoundingRadius)
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
        public static void FlashE()
        {
            if (!Program.emenu.Item("FlashE").GetValue<KeyBind>().Active) return;

            var positions = GetRotatedFlashPositions();

            foreach (var p in positions)
            {
                var condemnUnit = CondemnCheck(p);
                if (condemnUnit != null)
                {
                    Program.E.Cast(condemnUnit);

                    ObjectManager.Player.Spellbook.CastSpell(FlashSlot, p);

                }
            }
        }

        public static int CountHerosInRange(Obj_AI_Hero target, bool checkteam, float range = 1200f)
        {
            var objListTeam =
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(
                        x => x.IsValidTarget(range));

            return objListTeam.Count(hero => checkteam ? hero.Team != target.Team : hero.Team == target.Team);
        }

        public static Vector3 GetFirstNonWallPos(Vector3 startPos, Vector3 endPos)
        {
            int distance = 0;
            for (int i = 0; i < Program.emenu.Item("PushDistance").GetValue<Slider>().Value; i += 20)
            {
                var cell = startPos.Extend(endPos, endPos.Distance(startPos) + i);
                if (NavMesh.GetCollisionFlags(cell).HasFlag(CollisionFlags.Wall) ||
                    NavMesh.GetCollisionFlags(cell).HasFlag(CollisionFlags.Building))
                {
                    distance = i - 20;
                }
            }
            return startPos.Extend(endPos, distance + endPos.Distance(startPos));
        }

        public static List<Vector3> GetRotatedFlashPositions()
        {
            const int currentStep = 30;
            var direction = ObjectManager.Player.Direction.To2D().Perpendicular();

            var list = new List<Vector3>();
            for (var i = -90; i <= 90; i += currentStep)
            {
                var angleRad = Geometry.DegreeToRadian(i);
                var rotatedPosition = ObjectManager.Player.Position.To2D() + (425f * direction.Rotated(angleRad));
                list.Add(rotatedPosition.To3D());
            }
            return list;
        }
        public static void LoadFlash()
        {
            var testSlot = ObjectManager.Player.GetSpellSlot("SummonerFlash");
            if (testSlot != SpellSlot.Unknown)
            {
                Console.WriteLine("Flash Slot: {0}", testSlot);
                FlashSlot = testSlot;
            }
            else
            {
                Console.WriteLine("Error loading Flash! Not found!");
            }
        }

        public static Vector3 GetFlashPos(Obj_AI_Hero target, bool serverPos, int distance = 150)
        {
            var enemyPos = serverPos ? target.ServerPosition : target.Position;
            var myPos = serverPos ? ObjectManager.Player.ServerPosition : ObjectManager.Player.Position;

            return enemyPos + Vector3.Normalize(enemyPos - myPos) * distance;
        }

        public static Obj_AI_Hero CondemnCheck(Vector3 fromPosition)
        {
            var HeroList = HeroManager.Enemies.Where(
                h =>
                    h.IsValidTarget(Program.E.Range) &&
                    !h.HasBuffOfType(BuffType.SpellShield) &&
                    !h.HasBuffOfType(BuffType.SpellImmunity));
            foreach (var Hero in HeroList)
            {
                var ePred = Program.E2.GetPrediction(Hero);
                int pushDist = Program.emenu.Item("PushDistance").GetValue<Slider>().Value;
                for (int i = 0; i < pushDist; i += (int)Hero.BoundingRadius)
                {
                    Vector3 loc3 = ePred.UnitPosition.To2D().Extend(fromPosition.To2D(), -i).To3D();
                    var collFlags = NavMesh.GetCollisionFlags(loc3);
                    if (collFlags.HasFlag(CollisionFlags.Wall) || collFlags.HasFlag(CollisionFlags.Building))
                    {
                        return Hero;
                    }
                }
            }
            return null;
        }

    }
}
