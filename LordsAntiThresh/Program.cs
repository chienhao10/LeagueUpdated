using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace LordsAntiThresh
{
    class Program
    {
        static Menu Menu;
        static Spell Spell;
        static Items.Item _stealthWard, _visionWard, _Trinket, _SightStone, _RSightStone , _zzRot, _StealthV, _StealthG, _Eyeofthe, _EyeoftheO, _EyeoftheW, _FSight;
        static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }
        static bool ThreshInGame
        {
            get
            {
                return ObjectManager.Get<Obj_AI_Hero>().Any(h => h.IsEnemy && !h.IsMe && h.ChampionName.Equals("Thresh"));
            }
        }
        static Obj_AI_Base Lantern
        {
            get
            {
                return ObjectManager.Get<Obj_AI_Base>().FirstOrDefault(o => o.IsValid && o.IsEnemy && o.Name.Equals("ThreshLantern") 
                    /*&& Player.Distance(o) <= 500*/);
            }
        }

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Game.PrintChat("<font size='30'>LordsAntiThresh</font> <font color='#b756c5'>by LordZEDith</font>");
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (!ThreshInGame) return;

            Menu = new Menu("Anti Lantern", "AntiLantern", true);
            Menu alMenu = new Menu("On/Off", "OnOff");
            alMenu.AddItem(new MenuItem("alPress", "Enable")).SetValue<KeyBind>(new KeyBind('H', KeyBindType.Press));
            Menu.AddSubMenu(alMenu);
            Menu.AddToMainMenu();

            switch (ObjectManager.Player.CharData.BaseSkinName)
            {
                case "Teemo" :
                    InitSpell(SpellSlot.R, 230f, TargetSelector.DamageType.Magical);
                    break;
                case "Shaco" :
                    InitSpell(SpellSlot.W, 425f, TargetSelector.DamageType.Magical);
                    break;
                case "Zyra" :
                    InitSpell(SpellSlot.W, 850f, TargetSelector.DamageType.Magical);
                    break;
                case "Azir" :
                    InitSpell(SpellSlot.W, 450f, TargetSelector.DamageType.Magical);
                    break;
                case "Thresh" :
                    InitSpell(SpellSlot.W, 950f, TargetSelector.DamageType.Magical);
                    break;
                case "Jhin":
                    InitSpell(SpellSlot.E, 750f, TargetSelector.DamageType.Magical);
                    break;
                case "Heimerdinger":
                    InitSpell(SpellSlot.Q, 450f, TargetSelector.DamageType.Magical);
                    break;
                case "Veigar":
                    InitSpell(SpellSlot.W, 700f, TargetSelector.DamageType.Magical);
                    break;
                case "Aurelion Sol":
                    InitSpell(SpellSlot.Q, 650f, TargetSelector.DamageType.Magical);
                    break;
                case "Janna":
                    InitSpell(SpellSlot.Q, 850f, TargetSelector.DamageType.Magical);
                    break;
                default :
                    break;
            }

            _stealthWard = new Items.Item(3361, 600f);
            _visionWard = new Items.Item(3362, 600f);
            _Trinket = new Items.Item(3340, 600f);

            _SightStone = new Items.Item(2049, 600f);
            _RSightStone = new Items.Item(2045, 600f);
            _FSight = new Items.Item(3363, 4000f);
            _zzRot = new Items.Item(3512, 100f);
            _StealthV = new Items.Item(3362, 600f);
            _StealthG = new Items.Item(3361, 600f);
            _Eyeofthe = new Items.Item(2303, 600f);
            _EyeoftheO = new Items.Item(2302, 600f);
            _EyeoftheW = new Items.Item(2301, 600f);

            Game.OnUpdate += Game_OnUpdate;

            Notifications.AddNotification("AntiLantern - Loaded", 5);
        }

        static bool Casted(Vector3 lanternPos)
        {
            var list = ObjectManager.Get<Obj_AI_Base>().Where(o => o.Position == lanternPos || o.ServerPosition == lanternPos);
            foreach (var item in list)
            {
                if (item.Name.Contains("ward")) return true;
            }
            return false;
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (!Menu.SubMenu("OnOff").Item("alPress").GetValue<KeyBind>().Active)
                return;

            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (Lantern == null) return;
            if (Casted(Lantern.ServerPosition))
            {
                Console.WriteLine("Already have ward");
                return;
            }

            if (!CastSpell(Lantern.Position))
                CastWard(Lantern.Position);
        }

        static void InitSpell(SpellSlot slot, float range,TargetSelector.DamageType type)
        {
            Spell = new Spell(slot, range, type);
        }

        static bool CastSpell(Vector3 lanternPos)
        {
            if (Spell != null && Spell.IsReady() && lanternPos.Distance(Player.ServerPosition) <= Spell.Range)
            {
                Console.WriteLine("Cast spell to lantern");
                Spell.Cast(lanternPos);
                return true;
            }
            return false;
        }

        static void CastWard(Vector3 lanternPos)
        {
            Console.WriteLine("Cast ward/Portal");
            if (_Trinket.IsReady() && _Trinket.IsInRange(lanternPos))
            {
                _Trinket.Cast(lanternPos);
                return;
            }

            if (_stealthWard.IsReady() && _stealthWard.IsInRange(lanternPos))
            {
                _stealthWard.Cast(lanternPos);
                return;
            }

            if (_visionWard.IsReady() && _visionWard.IsInRange(lanternPos))
            {
                _visionWard.Cast(lanternPos);
                return;
            }

            if (_SightStone.IsReady() && _SightStone.IsInRange(lanternPos))
            {
                _SightStone.Cast(lanternPos);
                return;
            }
    
            if (_RSightStone.IsReady() && _RSightStone.IsInRange(lanternPos))
            {
                _RSightStone.Cast(lanternPos);
                 return;
            }

           if (_Eyeofthe.IsReady() && _Eyeofthe.IsInRange(lanternPos))
           {
               _Eyeofthe.Cast(lanternPos);
               return;             
           }
            if (_EyeoftheO.IsReady() && _EyeoftheO.IsInRange(lanternPos))
            {
                _EyeoftheO.Cast(lanternPos);
                return;
            }

            if (_EyeoftheW.IsReady() && _EyeoftheW.IsInRange(lanternPos))
            {
                _EyeoftheW.Cast(lanternPos);
                return;               
            }
            if (_StealthG.IsReady() && _StealthG.IsInRange(lanternPos))
            {
                _StealthG.Cast(lanternPos);
                return;
            }
            if (_StealthV.IsReady() && _StealthV.IsInRange(lanternPos))
            {
                _StealthV.Cast(lanternPos);
                return;
            }
            if (_FSight.IsReady() && _FSight.IsInRange(lanternPos))
            {
               _FSight.Cast(lanternPos);
                return;
            }
            if (_zzRot.IsReady() && _zzRot.IsInRange(lanternPos))
            {
                _zzRot.Cast(lanternPos);
              
            }
        }
    }
}
