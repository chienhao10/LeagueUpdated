using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace ToasterLord.Miscs
{
    class Misc
    {
        public static Menu.MenuItemSettings Miscs = new Menu.MenuItemSettings();

        private Misc()
        {

        }

        ~Misc()
        {
            
        }

        private static void SetupMainMenu()
        {
            var menu = new LeagueSharp.Common.Menu("SMiscs", "ToasterLordSMiscs", true);
            SetupMenu(menu);
            menu.AddToMainMenu();
        }

        public static Menu.MenuItemSettings SetupMenu(LeagueSharp.Common.Menu menu, bool useExisitingMenu = false)
        {
            Language.SetLanguage();
            if (!useExisitingMenu)
            {
                Miscs.Menu = menu.AddSubMenu(new LeagueSharp.Common.Menu(Language.GetString("MISCS_MISC_MAIN"), "ToasterLordMiscs"));
            }
            else
            {
                Miscs.Menu = menu;
            }
            if (!useExisitingMenu)
            {
                Miscs.MenuItems.Add(Miscs.CreateActiveMenuItem("ToasterLordMiscsActive"));
            }
            return Miscs;
        }
    }
}
