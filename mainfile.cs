using System;
using System.Diagnostics;

namespace Firewall
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to the firewall using the netsh command
            ProcessStartInfo connectInfo = new ProcessStartInfo();
            connectInfo.FileName = "netsh";
            connectInfo.Arguments = "advfirewall set allprofiles state on";
            connectInfo.UseShellExecute = false;
            connectInfo.RedirectStandardOutput = true;
            Process connectProcess = Process.Start(connectInfo);
            connectProcess.WaitForExit();
            string connectOutput = connectProcess.StandardOutput.ReadToEnd();
            if (connectOutput.Contains("Ok."))
            {
                Console.WriteLine("Successfully connected to firewall");
            }
            else
            {
                Console.WriteLine("Error: Could not connect to firewall");
                return;
            }

            // Listen for incoming and outgoing traffic using the netsh command
            ProcessStartInfo listenInfo = new ProcessStartInfo();
            listenInfo.FileName = "netsh";
            listenInfo.Arguments = "advfirewall firewall add rule name=TrafficRule dir=in action=allow";
            listenInfo.UseShellExecute = false;
            listenInfo.RedirectStandardOutput = true;
            Process listenProcess = Process.Start(listenInfo);
            listenProcess.WaitForExit();
            string listenOutput = listenProcess.StandardOutput.ReadToEnd();
            if (listenOutput.Contains("Ok."))
            {
                Console.WriteLine("Listening for incoming traffic...");
            }
            else
            {
                Console.WriteLine("Error: Could not listen for incoming traffic");
                return;
            }

            listenInfo.Arguments = "advfirewall firewall add rule name=TrafficRule dir=out action=allow";
            listenProcess = Process.Start(listenInfo);
            listenProcess.WaitForExit();
            listenOutput = listenProcess.StandardOutput.ReadToEnd();
            if (listenOutput.Contains("Ok."))
            {
                Console.WriteLine("Listening for outgoing traffic...");
            }
            else
            {
                Console.WriteLine("Error: Could not listen for outgoing traffic");
                return;
            }

            // Filter traffic based on a user-specified condition
            Console.WriteLine("Enter a condition to filter traffic (e.g. \"tcp from any to any 80\"): ");
            string condition = Console.ReadLine();
            ProcessStartInfo filterInfo = new ProcessStartInfo();
            filterInfo.FileName = "netsh";
            filterInfo.Arguments = "advfirewall firewall add rule name=FilterRule dir=in action=block " + condition;
            filterInfo.UseShellExecute = false;
            filterInfo.RedirectStandardOutput = true;
            Process filterProcess = Process.Start(filterInfo);
            filterProcess.WaitForExit();
            string filterOutput = filterProcess.StandardOutput.ReadToEnd();
            if (filterOutput.Contains("Ok."))
            {
                Console.WriteLine("Filtering incoming traffic based on condition: " + condition);
            }
            else
            {
                Console.WriteLine("Error: Could not filter incoming traffic");
                return;
            }

            filterInfo.Arguments = "advfirewall firewall add rule name=FilterRule dir=out action=block " + condition;
            filterProcess = Process.Start(filterInfo);
            filterProcess.WaitForExit();
            filterOutput = filterProcess.StandardOutput.ReadToEnd();
            if (filterOutput.Contains("Ok."))
            {
                Console.WriteLine("Filtering outgoing traffic based on condition: " + condition);
            }
            else
            {
                Console.WriteLine("Error: Could not filter outgoing traffic");
                return;
            }
        }
    }
}
//end of module