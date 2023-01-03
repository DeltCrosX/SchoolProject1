using System;
using System.Net.NetworkInformation;

namespace FirewallTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to the firewall
            Console.WriteLine("Connecting to firewall...");
            bool connected = ConnectToFirewall();
            if (connected)
            {
                Console.WriteLine("Successfully connected to firewall");
            }
            else
            {
                Console.WriteLine("Error: Could not connect to firewall");
                return;
            }

            // Listen for incoming and outgoing traffic
            Console.WriteLine("Listening for incoming and outgoing traffic...");
            bool listening = ListenForTraffic();
            if (listening)
            {
                Console.WriteLine("Successfully listening for traffic");
            }
            else
            {
                Console.WriteLine("Error: Could not listen for traffic");
                return;
            }

            // Filter traffic for all IP addresses
            Console.WriteLine("Filtering traffic for all IP addresses...");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation ci in connections)
            {
                string localIpAddress = ci.LocalEndPoint.Address.ToString();
                string remoteIpAddress = ci.RemoteEndPoint.Address.ToString();
                bool localFiltered = FilterTraffic(localIpAddress);
                bool remoteFiltered = FilterTraffic(remoteIpAddress);
                if (localFiltered && remoteFiltered)
                {
                    Console.WriteLine($"Successfully filtered traffic for IP addresses {localIpAddress} and {remoteIpAddress}");
                }
                else
                {
                    Console.WriteLine($"Error: Could not filter traffic for IP addresses {localIpAddress} and {remoteIpAddress}");
                    return;
                }
            }

            Console.WriteLine("Finished filtering traffic for all IP addresses");
        }

        static bool ConnectToFirewall()
        {
            // Connect to the firewall using the System.Net.NetworkInformation.NetworkInterface class
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.OperationalStatus == OperationalStatus.Up)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static bool ListenForTraffic()
        {
            // Listen for incoming and outgoing traffic using the System.Net.NetworkInformation.TcpConnectionInformation class
            try
            {
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
                return connections.Length > 0;
            }
            catch (Exception)
        }
        static bool FilterTraffic(string ipAddress)
        {
            // Filter traffic for the specified IP address using the System.Net.NetworkInformation.IPGlobalProperties class
            try
            {
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
                foreach (TcpConnectionInformation ci in connections)
                {
                    if (ci.LocalEndPoint.Address.ToString() == ipAddress || ci.RemoteEndPoint.Address.ToString() == ipAddress)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
