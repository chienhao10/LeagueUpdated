using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using ToasterLord;
using Menu = ToasterLord.Menu;

namespace ToasterLord
{
    class ToasterLord
    {
        public static Menu.MenuItemSettings ToasterLordMisc = new Menu.MenuItemSettings(typeof(ToasterLord));

        private MemoryStream packet;
        private bool packetSent = false;
        private Notification notification = null;
        private Notification notificationRemaining = null;
        private long ms = -1;
        private DrawingDraw drawingEvent = null;
        private int delay = 420;

        public ToasterLord()
        {
            if (Game.Mode == GameMode.Running)
            {
                return;
            }
            SetupMenu();
            ToasterLordMisc.GetMenuItem("ToasterLordMiscsToasterLordActive").ValueChanged += Active_OnValueChanged;
            notification = Common.ShowNotification("Waiting for the packet", Color.LawnGreen, -1);
            Game.OnSendPacket += Game_OnSendPacket;
            Game.OnWndProc += Game_OnWndProc;
            GameUpdate updateEvent = null;
            updateEvent = delegate
            {
                if (Game.Mode == GameMode.Running)
                {
                    LeagueSharp.Common.Menu.RootMenus.Remove(Assembly.GetCallingAssembly().GetName().Name + "." + ToasterLordMisc.Menu.Name);
                    Game.OnUpdate -= updateEvent;
                }
            };
            
        }

        ~ToasterLord()
        {
            Game.OnSendPacket -= Game_OnSendPacket;
            Game.OnWndProc -= Game_OnWndProc;
            if (notification != null)
            {
                Notifications.RemoveNotification(notification);
                notification.Dispose();
            }
            if (notificationRemaining != null)
            {
                Notifications.RemoveNotification(notificationRemaining);
                notificationRemaining.Dispose();
            }
        }

        public bool IsActive()
        {
            return ToasterLordMisc.GetActive();
        }

        public static Menu.MenuItemSettings SetupMenu()
        {
            Language.SetLanguage();
            ToasterLordMisc.Menu = new LeagueSharp.Common.Menu("ToasterLord", "ToasterLord", true);
            ToasterLordMisc.MenuItems.Add(
                ToasterLordMisc.Menu.AddItem(new MenuItem("ToasterLordMiscsToasterLordActive", Language.GetString("GLOBAL_ACTIVE")).SetValue(true)));
            ToasterLordMisc.Menu.AddItem(new MenuItem("By LordZEDith", "By LordZEDith" + Assembly.GetExecutingAssembly().GetName().Version));
            ToasterLordMisc.Menu.AddToMainMenu();
            return ToasterLordMisc;
        }

        private void Active_OnValueChanged(object sender, OnValueChangeEventArgs onValueChangeEventArgs)
        {
            Game.OnSendPacket -= Game_OnSendPacket;
            Game.OnWndProc -= Game_OnWndProc;
            if (packet != null)
            {
                Game.SendPacket(packet.ToArray(), PacketChannel.C2S, PacketProtocolFlags.Reliable);
            }
            if (notification != null)
            {
                Notifications.RemoveNotification(notification);
                notification.Dispose();
            }
            if (notificationRemaining != null)
            {
                Notifications.RemoveNotification(notificationRemaining);
                notificationRemaining.Dispose();
            }
        }

        private void Game_OnSendPacket(GamePacketEventArgs args)
        {
            try
            {
                if (Game.Mode != GameMode.Running || !IsActive())
                {
                    //Console.Write("Packet Sent: ");
                    //args.PacketData.ForEach(x => Console.Write(x + " "));
                    //Console.WriteLine();
                    if (args.PacketData.Length != 6 || packetSent || packet != null)
                        return;
                    args.Process = false;
                    packet = new MemoryStream(args.PacketData, 0, args.PacketData.Length);
                    Notifications.RemoveNotification(notification);
                    notification.Dispose();
                    notification = Common.ShowNotification("Press spacebar to continue.", Color.YellowGreen, -1);
                    ms = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    drawingEvent = delegate
                    {
                        if (!packetSent && (ms + (delay * 1000)) < (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond))
                        {
                            Game.SendPacket(packet.ToArray(), PacketChannel.C2S, PacketProtocolFlags.Reliable);
                            packetSent = true;
                            if (notification != null)
                            {
                                Notifications.RemoveNotification(notification);
                                notification.Dispose();
                            }
                            if (notificationRemaining != null)
                            {
                                Notifications.RemoveNotification(notificationRemaining);
                                notificationRemaining.Dispose();
                            }
                            notification = Common.ShowNotification("Game starts now. Prepare!", Color.OrangeRed, 1000);
                            Drawing.OnDraw -= drawingEvent;
                        }
                        else
                        {
                            if (notificationRemaining == null)
                            {
                                notificationRemaining =
                                Common.ShowNotification(
                                    "Remaining: " +
                                    (ms + (delay * 1000) - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString(),
                                    Color.YellowGreen, delay * 1000);
                            }
                            else
                            {
                                notificationRemaining.Text = "Remaining: " +
                                    ((ms + (delay * 1000) - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) / 1000).ToString() + "s";
                            }
                        }
                    };
                    Drawing.OnDraw += drawingEvent;
                }
                else
                {
                    if (notification != null)
                    {
                        Notifications.RemoveNotification(notification);
                        notification.Dispose();
                    }
                    if (notificationRemaining != null)
                    {
                        Notifications.RemoveNotification(notificationRemaining);
                        notificationRemaining.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Game_OnWndProc(WndEventArgs args)
        {
            if ((WindowsMessages)args.Msg != WindowsMessages.WM_KEYUP || args.WParam != 32 || packetSent)
                return;
            Game.SendPacket(packet.ToArray(), PacketChannel.C2S, PacketProtocolFlags.Reliable);
            packetSent = true;
            if (notification != null)
            {
                Notifications.RemoveNotification(notification);
                notification.Dispose();
            }
            if (notificationRemaining != null)
            {
                Notifications.RemoveNotification(notificationRemaining);
                notificationRemaining.Dispose();
            }
            if (drawingEvent != null)
            {
                Drawing.OnDraw -= drawingEvent;
            }
            notification = Common.ShowNotification("Game starts now. Prepare!", Color.OrangeRed, 1000);
        }
    }
}
