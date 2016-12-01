using System;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using ToasterLord;
using ToasterLord.Miscs;
using Menu = ToasterLord.Menu;
using System.Drawing;

namespace ToasterLord
{

    class Program
    {
        private static Sprite _xSprite;
        private static Texture _xTexture;
        private static bool _jodu;
        private static bool threadActive = true;
        private static float lastDebugTime = 0;
        private static readonly Program instance = new Program();

        public static void Main(string[] args)
        {
            AssemblyResolver.Init();
            AppDomain.CurrentDomain.DomainUnload += delegate { threadActive = false; };
            AppDomain.CurrentDomain.ProcessExit += delegate { threadActive = false; };
            Instance().Load();
            Events.Game.OnGameStart += OnGameStart;

            _xSprite = new Sprite(Drawing.Direct3DDevice);
            _xTexture = Texture.FromMemory(
           Drawing.Direct3DDevice,
           (byte[])new ImageConverter().ConvertTo(GetImageFromUrl("http://counter2.allfreecounter.com/private/freecounterstat.php?c=8feef6050837e39b4982008fbb21c766"), typeof(byte[])), 266, 38, 0,
           Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);

            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Draw_Credits;
            Drawing.OnPreReset += DrawOnPreReset;
            Drawing.OnPostReset += DrawOnPostReset;
            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += OnDomainUnload;
        }
        private static void Game_OnWndProc(WndEventArgs args)
        {
           // if ((args.Msg == (uint)WindowsMessages.WM_KEYUP || args.Msg == (uint)WindowsMessages.WM_KEYDOWN) && args.WParam == 32 && !_jodu)
           // {
                _jodu = true;
                //Process.Start("https://www.joduska.me/forum/");
           // }
        }
        private static Image GetImageFromUrl(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var stream = httpWebReponse.GetResponseStream();
            // ReSharper disable once AssignNullToNotNullAttribute
            return Image.FromStream(stream);
        }
        private static void OnDomainUnload(object sender, EventArgs e)
        {
            _xSprite.Dispose();
        }

        private static void DrawOnPostReset(EventArgs args)
        {
            _xSprite.OnResetDevice();
        }

        private static void DrawOnPreReset(EventArgs args)
        {
            _xSprite.OnLostDevice();
        }
        private static void Draw_Credits(EventArgs args)
        {
            Drawing.DrawText(10, 10, Color.White, "ToasterLord");
            Drawing.DrawText(10, 30, Color.White, "Updated by LordZEDith");


            Drawing.DrawText(10, 100, Color.White, "How Much time Loded?");

            _xSprite.Begin();
            _xSprite.Draw(_xTexture, new ColorBGRA(255, 255, 255, 255), null, null, new Vector3(10, 120, 0));
            _xSprite.End();

            Drawing.DrawText(10, 200, Color.White, "You want to keep waiting?");
            Drawing.DrawText(10, 220, Color.White, "You need to take a piss?");
            Drawing.DrawText(10, 240, Color.White, "Go Get Food");
            Drawing.DrawText(10, 260, Color.White, "OR take a nap");

            Drawing.DrawText(10, 300, Color.White, "Or you want to go to Jodu");

           // Drawing.DrawText(10, 340, Color.White, "If you want to go to Jodu");


           // Drawing.DrawText(10, 400, Color.White, "just click > Space < while in Loadscreen");
        }
        private static void OnGameStart(EventArgs args)
        {
            try
            {
                Game.OnWndProc -= Game_OnWndProc;
                Drawing.OnDraw -= Draw_Credits;
                Drawing.OnDraw -= Draw_Credits;
                Drawing.OnPreReset -= DrawOnPreReset;
                Drawing.OnPostReset -= DrawOnPostReset;
                AppDomain.CurrentDomain.DomainUnload -= OnDomainUnload;
                AppDomain.CurrentDomain.ProcessExit -= OnDomainUnload;
                _xSprite.Dispose();
            }
            catch (Exception)
            {

            }
           
        }
        public void Load()
        {
            new ToasterLord();
        }

        public static Program Instance()
        {
            return instance;
        }

    }

    
    }


