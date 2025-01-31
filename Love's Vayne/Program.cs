﻿
using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Lord_s_Vayne
{
    class Program
    {
        public static Spell E;
        public static Spell E2;
        public static Spell Q;
        public static Spell W;
        public static Spell R;




        public static Vector3 TumblePosition = Vector3.Zero;


        public static MyOrbwalker.Orbwalker orbwalker;

        private static string News = "Welcome to Love's Vayne";

        public static Menu menu;

        public static Dictionary<string, SpellSlot> spellData;
        public static Items.Item zzrot = new Items.Item(3512, 400);

        public static Obj_AI_Hero tar;
        public const string ChampName = "Vayne";
        public static Obj_AI_Hero Player;

        //public static Menu Itemsmenu;
        public static Menu qmenu;
        public static Menu emenu;
        public static Menu gmenu;
        public static Menu imenu;
        public static Menu rmenu;
       
    


        public static float LastMoveC;

        public static void MoveToLimited(Vector3 where)
        {
            if (Environment.TickCount - LastMoveC < 80)
            {
                return;
            }

            LastMoveC = Environment.TickCount;
            Player.IssueOrder(GameObjectOrder.MoveTo, where);
        }

        public static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
        }
        #region GameOnLoad
       
            public static void Game_OnGameLoad(EventArgs args)
            {
                Program.Q = new Spell(SpellSlot.Q, 300f);
                Program.W = new Spell(SpellSlot.W);
                Program.E2 = new Spell(
                   SpellSlot.E, (uint)(650 + ObjectManager.Player.BoundingRadius));
                Program.E = new Spell(SpellSlot.E, 550f);
                Program.E.SetTargetted(0.25f, 1600f);
                Program.R = new Spell(SpellSlot.R);

                Program.Player = ObjectManager.Player;
     
                if (Program.Player.ChampionName != ChampName) return;
                Program.spellData = new Dictionary<string, SpellSlot>();
 
                Program.menu = new Menu("Love's Vayne", "Love's Vayne", true);
            
                Program.menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
                Program.orbwalker = new MyOrbwalker.Orbwalker(Program.menu.SubMenu("Orbwalker"));
             
                var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
                TargetSelector.AddToMenu(TargetSelectorMenu);
               
                Program.menu.AddSubMenu(TargetSelectorMenu);

                Program.menu.AddItem(
                    new MenuItem("aaqaa", "Auto -> Q -> AA").SetValue(new KeyBind("N".ToCharArray()[0], KeyBindType.Press)));

                Program.qmenu = Program.menu.AddSubMenu(new Menu("Tumble", "Tumble"));
                Program.qmenu.AddItem(new MenuItem("FastQ", "Fast Q").SetValue(true).SetValue(true).SetTooltip("Q Animation Cancelation"));
                Program.qmenu.AddItem(new MenuItem("UseQC", "Use Q Combo").SetValue(true));
                //Program.qmenu.AddItem(new MenuItem("QMode", "Use Q Mode:", true).SetValue(new StringList(new[] { "Gosu", "Cursor Position" })));
                Program.qmenu.AddItem(new MenuItem("hq", "Use Q Harass").SetValue(true));
                Program.qmenu.AddItem(new MenuItem("restrictq", "Restrict Q usage?").SetValue(true));
                Program.qmenu.AddItem(new MenuItem("UseQJ", "Use Q Farm").SetValue(true));                           
                Program.qmenu.AddItem(new MenuItem("Junglemana", "Minimum Mana to Use Q Farm").SetValue(new Slider(60, 1, 100)));
                Program.qmenu.AddItem(new MenuItem("AntiMQ", "Use Anti - Melee [Q]").SetValue(true));             
                Program.qmenu.AddItem(new MenuItem("FocusTwoW", "Focus 2 W Stacks").SetValue(true));
                //qmenu.AddItem(new MenuItem("DrawQ", "Draw Q Arrow").SetValue(true));


                Program.emenu = Program.menu.AddSubMenu(new Menu("Condemn", "Condemn"));
                Program.emenu.AddItem(new MenuItem("UseEC", "Use E Combo").SetValue(true));
                Program.emenu.AddItem(new MenuItem("he", "Use E Harass").SetValue(false));
                Program.emenu.AddItem(new MenuItem("UseET", "Use E (Toggle)").SetValue(new KeyBind("L".ToCharArray()[0], KeyBindType.Toggle)));
                Program.emenu.AddItem(new MenuItem("zzrot", "[Beta] ZZrot Condemn").SetValue(new KeyBind("I".ToCharArray()[0], KeyBindType.Toggle))).Permashow(true, "Vayne | ZZRot Toggle", Color.Aqua);
                // emenu.AddItem(new MenuItem("FlashE", "Flash E").SetValue(true).SetValue(new KeyBind("Y".ToCharArray()[0], KeyBindType.Press)));


                //emenu.AddItem(new MenuItem("Gap_E", "Use E To Gabcloser").SetValue(true));
                // emenu.AddItem(new MenuItem("GapD", "Anti GapCloser Delay").SetValue(new Slider(0, 0, 1000)).SetTooltip("Sets a delay before the Condemn for Antigapcloser is casted."));
                Program.emenu.AddItem(new MenuItem("EMode", "Use E Mode:", true).SetValue(new StringList(new[] { "Love's", "Gosu", "Flowers", "VHR", "Marksman", "Sharpshooter", "OKTW", "Shine", "PRADASMART", "PRADAPERFECT", "OLDPRADA", "PRADALEGACY" })));
                Program.emenu.AddItem(new MenuItem("PushDistance", "E Push Distance").SetValue(new Slider(415, 475, 300)));
                Program.emenu.AddItem(new MenuItem("EHitchance", "E Hitchance").SetValue(new Slider(50, 1, 100)).SetTooltip("Only use this for Prada Condemn Methods"));
                Program.emenu.AddItem(new MenuItem("UseEaa", "Use E after auto").SetValue(new KeyBind("E".ToCharArray()[0], KeyBindType.Press)));


            Program.rmenu = Program.menu.AddSubMenu(new Menu("Ult", "Ult"));
                Program.rmenu.AddItem(new MenuItem("visibleR", "Smart Invisible R").SetValue(true).SetTooltip("Wether you want to set a delay to stay in R before you Q"));
                Program.rmenu.AddItem(new MenuItem("Qtime", "Duration to wait").SetValue(new Slider(700, 0, 1000)));

                Program.imenu = Program.menu.AddSubMenu(new Menu("Interrupt Settings", "Interrupt Settings"));
                Program.imenu.AddItem(new MenuItem("Int_E", "Use E To Interrupt").SetValue(true));
                Program.imenu.AddItem(new MenuItem("Interrupt", "Interrupt Danger Spells", true).SetValue(true));
                Program.imenu.AddItem(new MenuItem("AntiAlistar", "Interrupt Alistar W", true).SetValue(true));
                Program.imenu.AddItem(new MenuItem("AntiRengar", "Interrupt Rengar Jump", true).SetValue(true));
                Program.imenu.AddItem(new MenuItem("AntiKhazix", "Interrupt Khazix R", false).SetValue(false));
                Program.imenu.AddItem(new MenuItem("AntiKhazix", "Interrupt Khazix R", false).SetValue(false));


                Program.gmenu = Program.menu.AddSubMenu(new Menu("Gap Closer", "Gap Closer"));
                Program.gmenu.AddItem(new MenuItem("Gapcloser", "Anti Gapcloser", true).SetValue(false));
                foreach (var target in HeroManager.Enemies)
                {
                    Program.gmenu.AddItem(
                        new MenuItem("AntiGapcloser" + target.ChampionName.ToLower(), target.ChampionName, true)
                            .SetValue(false));
                }


                
                Program.menu.AddItem(new MenuItem("useR", "Use R Combo").SetValue(true));
                Program.menu.AddItem(new MenuItem("enemys", "If Enemys Around >=").SetValue(new Slider(2, 1, 5)));



                Q = new Spell(SpellSlot.Q, 0f);
                R = new Spell(SpellSlot.R, float.MaxValue);
                E = new Spell(SpellSlot.E, 650f);
                E.SetTargetted(0.25f, 1600f);

               

                E.SetTargetted(0.25f, 2200f);
                Obj_AI_Base.OnProcessSpellCast += Events.Game_SpellProcess.Game_ProcessSpell;
                Game.OnUpdate += Events.GameUpdate.Game_OnGameUpdate;
                MyOrbwalker.AfterAttack += Events.AfterAttack.Orbwalking_AfterAttack;
                MyOrbwalker.BeforeAttack += Events.BeforeAttack.Orbwalking_BeforeAttack;
                AntiGapcloser.OnEnemyGapcloser += Events.AntiGapCloser.AntiGapcloser_OnEnemyGapcloser;
                Interrupter2.OnInterruptableTarget += Events.Interrupter.Interrupter2_OnInterruptableTarget;
                Obj_AI_Base.OnPlayAnimation += Events.Play.OnPlay;
                GameObject.OnCreate += Events.OnCreates.OnCreate;
                //  Drawing.OnDraw += DrawingOnOnDraw;


                //Game.PrintChat("<font color='#881df2'>Blm95 Vayne Reborn by Lovetaiwan</font> Loaded.");
                Game.PrintChat("<font size='30'>Love's Vayne</font> <font color='#b756c5'>by LordZEDith</font>");
                Game.PrintChat("<font color='#b756c5'>NEWS: </font>" + Program.News);
                //Game.PrintChat(
                // "<font color='#f2f21d'>Do you like it???  </font> <font color='#ff1900'>Drop 1 Upvote in Database </font>");
                Game.PrintChat(
                "<font color='#f2f21d'>Buy me cigars </font> <font color='#ff1900'>ssssssssssmith@hotmail.com</font> (10) S");
                Program.menu.AddToMainMenu();
            }
        }
        #endregion
    }


