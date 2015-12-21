using System.Diagnostics;
using System.Net;

namespace Mumble_Status
{
    public class Data
    {
        public static byte[] mumbleResponse { set; get; }
        public static string serverVersion { set; get; } = "0.0.0";
        public static int maxUsers { set; get; } = 0;
        public static int lastUsers { set; get; } = 0;
        public static int currentUsers { set; get; } = 0;
        public static int requestTime { set; get; } = 10000;
        public static bool forceMute { set; get; } = false;
        public static bool debug { set; get; } = true;
        public static string lastUpdate { set; get; } = "00:00 AM";
    }

    class Functions
    {
        public static string GetHostname(string hostname)
        {
            string serverAddress = null;

            foreach (IPAddress host in Dns.GetHostAddresses(hostname))
            {
                serverAddress = host.ToString();
            }

            return serverAddress;
        }

        public static void GetMaxUsers()
        {
            Data.maxUsers = Data.mumbleResponse[19];
        }

        public static void GetServerVersion()
        {
            Data.serverVersion = string.Format("{0}.{1}.{2}", Data.mumbleResponse[1], Data.mumbleResponse[2], Data.mumbleResponse[3]);
        }

        public static void GetCurrentUsers()
        {
            Data.currentUsers = Data.mumbleResponse[15];
        }

        public static bool CheckMumble()
        {
            Process[] pname = Process.GetProcessesByName("mumble");
            if (pname.Length == 0)
                return false;
            else
                return true;
        }
    }
}
