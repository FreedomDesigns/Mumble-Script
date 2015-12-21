using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace Mumble_Status
{
    class UX
    {
        public static void CheckVersion()
        {
            string exeVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (exeVersion != GetGitVersion())
            {
                DialogResult dialogResult = MessageBox.Show("A New Update Is Available - Would you like to download now?", "New Update Available", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    Process.Start("https://github.com/FreedomDesigns/Mumble-Script/releases");
                    Environment.Exit(0);
                }
                else if (dialogResult == DialogResult.No)
                {
                    Environment.Exit(0);
                }
                
            }
        }

        private static string GetGitVersion()
        {
            try
            {
                return new WebClient().DownloadString("https://raw.githubusercontent.com/FreedomDesigns/Mumble-Script/master/version.txt");
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    Console.WriteLine(string.Format("Error {0} - Unable to get version number", resp.StatusCode));
                }
            }
            return null;
        }

        private static bool first = false;

        public static void Run()
        {
            while (true)
            {
                if (!Data.forceMute || !Functions.CheckMumble())
                {
                    /*
                    if (Data.debug)
                    {
                        UI.CreateToast("Mumble Server Information [Debug]", "Hello There, Mate", ToolTipIcon.Info);
                    }
                    */

                    Mumble.SendMumble();

                    if (Data.mumbleResponse.Length > 23)
                    {
                        //worked++;
                        Data.lastUpdate = DateTime.Now.ToString("HH:mm:ss tt");
                        Functions.GetCurrentUsers();

                        if (Data.currentUsers > Data.lastUsers)
                        {
                            Data.lastUsers = Data.currentUsers;

                            // Create notification for user has joined
                            UI.CreateToast("Mumble Server Information", "User has joined mumble [" + Data.currentUsers + " Clients]", ToolTipIcon.Info);
                        }
                        else if (Data.lastUsers < Data.currentUsers)
                        {
                            Data.lastUsers = Data.currentUsers;

                            // Create notification for user has joined
                            UI.CreateToast("Mumble Server Information", "User has left mumble [" + Data.currentUsers + " Clients]", ToolTipIcon.Info);
                        }

                        if (!first)
                        {
                            Functions.GetMaxUsers();
                            Functions.GetServerVersion();
                            first = true;
                        }
                    }
                }
                System.Threading.Thread.Sleep(Data.requestTime);
            }
        }
    }
}
