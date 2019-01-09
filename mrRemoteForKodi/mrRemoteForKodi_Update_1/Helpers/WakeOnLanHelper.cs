using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace mrRemoteForKodi_Update_1.Helpers
{
    public class WakeOnLanHelper
    {
        public static void WakeUp(string macAddress, string subnetMask, string port)
        {
            var client = new UdpClient();

            Byte[] datagram = new byte[102];

            for (int i = 0; i <= 5; i++)
            {
                datagram[i] = 0xff;
            }

            string[] macDigits = null;
            if (macAddress.Contains("-"))
            {
                macDigits = macAddress.Split('-');
            }
            else
            {
                macDigits = macAddress.Split(':');
            }

            var start = 6;
            for (int i = 0; i < 16; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                }
            }

            client.SendAsync(datagram, datagram.Length, subnetMask, Convert.ToInt32(port));
        }
    }
}