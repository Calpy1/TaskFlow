using System.Text;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

namespace TaskFlow.Services
{
    public class SecurityService
    {
        public static string HashPassword(string password)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password.Trim())));
        }

        public static string GetMac()
        {
            string mac = null;

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    if (networkInterface.OperationalStatus != OperationalStatus.Up)
                    {
                        continue;
                    }

                    byte[] addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();

                    if (addressBytes.Length == 0)
                    {
                        continue;
                    }

                    mac = BitConverter.ToString(addressBytes).Replace("-", ":");
                    break;
                }
            }

            return mac;
        }
    }
}
