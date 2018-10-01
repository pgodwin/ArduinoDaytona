using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoDaytonaClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
                ShowHelp();

            // No real checks, sends the command to the server blindly. 
            // Server will expect P1-P8, END
            try
            {
                var serverPort = args[0];
                var command = args[1];

                var server = serverPort.Split(':')[0];
                var port = int.Parse(serverPort.Split(':')[1]);

                var client = new SimpleTcpClient().Connect(server, port);
                client.WriteLine(command);
                client.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("appname.exe server-ip:port [END|1,2,3,4]");
            Console.WriteLine("eg appname.exe 127.0.0.1:8910 1");
            Console.WriteLine("Switches on Player 1's leader lamp.");

        }
    }
}
