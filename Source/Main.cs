using Mumble_Status.Properties;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Mumble_Status
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI());
        }
    }

    class UI : Form
    {
        private static NotifyIcon trayIcon;
        private static ContextMenu trayMenu;
        private Thread thread;

        public UI()
        {
            UX.CheckVersion();

            trayMenu = new ContextMenu();
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Mumble Status";
            trayIcon.Icon = new Icon(Resources.Icon, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.ContextMenu.Popup += CreateMenu;

            thread = new Thread(UX.Run);
            thread.Start();
        }

        private void CreateMenu(object sender, EventArgs e)
        {
            trayMenu.MenuItems.Clear();

            // Create server version
            var Version = new MenuItem();
            Version.Text = string.Format("Server Version - {0}", Data.serverVersion);
            Version.Enabled = false;
            trayMenu.MenuItems.Add(Version);

            // Create client count
            var client = new MenuItem();
            client.Text = string.Format("Current Users [{0}/{1}]", Data.currentUsers, Data.maxUsers);
            client.Enabled = false;
            trayMenu.MenuItems.Add(client);

            // Create server ping
            var Ping = new MenuItem();
            Ping.Text = string.Format("Ping - {0}ms", "N/A");
            Ping.Enabled = false;
            trayMenu.MenuItems.Add(Ping);

            if (Data.debug)
            {
                // Create wait text
                var Wait = new MenuItem();
                Wait.Text = string.Format("Interval - {0}s", Data.requestTime/1000);
                Wait.Enabled = false;
                trayMenu.MenuItems.Add(Wait);

                // Create availability text
                var Work = new MenuItem();
                Work.Text = string.Format("Availability - {0}%", "N/A"/*(worked / sent) * 100*/);
                Work.Enabled = false;
                trayMenu.MenuItems.Add(Work);

                // Create last uodate text
                var Update = new MenuItem();
                Update.Text = string.Format("Last Update - {0}", Data.lastUpdate);
                Update.Enabled = false;
                trayMenu.MenuItems.Add(Update);
            }

            trayMenu.MenuItems.Add("-");

            // Create notification options
            var mute = new MenuItem();

            if (!Data.forceMute)
            {
                mute.Checked = false;
                mute.Text = "Mute Notifications";
            }
            else if (Data.forceMute)
            {
                mute.Checked = true;
                mute.Text = "Un-Mute Notifications";
            }

            mute.Click += (s, a) => ForceMute();
            trayMenu.MenuItems.Add(mute);

            // Add an exit button
            var exitItem = new MenuItem { Text = "Exit" };
            exitItem.Click += OnExit;
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add(exitItem);

        }

        public static void CreateToast(string header, string message, ToolTipIcon icon)
        {
            trayIcon.ShowBalloonTip(3000, header, message, icon);
        }

        private void ForceMute()
        {
            if (Data.forceMute)
                Data.forceMute = false;
            else if (!Data.forceMute)
                Data.forceMute = true;
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(120, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Mumble";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
        }
    }
}
