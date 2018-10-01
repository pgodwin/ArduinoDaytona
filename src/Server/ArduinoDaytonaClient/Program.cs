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

            var serverPort = args[1];
            var command = args[2];

            var server = serverPort.Split(':')[0];
            var port = int.Parse(server.Split(':')[1]);

            var client = new SimpleTcpClient().Connect(server, port);
            client.WriteLine(command);
        }

        static void ShowHelp()
        {
            Console.WriteLine("appname.exe server-ip:port playernumber");
            Console.WriteLine("eg appname.exe 127.0.0.1:8910 2");

        }
    }
}
