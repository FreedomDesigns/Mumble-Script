using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mumble_Status
{
    class Mumble
    {
        public static void SendMumble()
        {
            // Try and catch for UDP fail
            try
            {
                byte[] sendBytes = new Byte[1024];

                UdpClient client = new UdpClient(AddressFamily.InterNetwork);

                client.Client.ReceiveTimeout = 20000;
                client.Client.SendTimeout = 20000;

                IPAddress address = IPAddress.Parse(Functions.GetHostname("absy.ddns.net"));
                client.Connect(address, 64738);
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                string data = "\x00\x00\x00\x00\x00\x78\x50\x61\x77\x2e\x6d\x65";
                sendBytes = Encoding.ASCII.GetBytes(data);
                client.Send(sendBytes, sendBytes.GetLength(0));

                var response = client.Receive(ref remoteIPEndPoint);
                client.Close();

                if (Data.requestTime != 10000)
                    Data.requestTime = 10000;

                Data.mumbleResponse = response;
            }
            catch
            {
                Data.requestTime = Data.requestTime + 5000;

                if (Data.debug)
                {
                    UI.CreateToast("Mumble Server Information", "UDP Request Failed", ToolTipIcon.Error);
                }

                Data.mumbleResponse =  new byte[] { 1, 7 };
            }
        }

        private void HandleMumble()
        {

        }
    }
}
