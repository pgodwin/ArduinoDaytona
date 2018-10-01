using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoDaytona
{
    class Program
    {
        static SerialPort serial;
        static SimpleTcpServer server;
        public static NotifyIcon notificationIcon;
        public static ContextMenu trayMenu = new ContextMenu();


        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 1)
                return;

            var comPort = args[0];

            // Note you won't see this app
            // The output type is windows app, so no console window will appear. 
            try
            {
                // Open the serial port
                serial = new SerialPort(comPort, 9600); //new GodSerialPort(comPort, 9600);
                
                serial.Open();
                serial.DtrEnable = true;

                // Write 0 to make sure it works. serial.open doesn't actually test anything.
                //serial.Write(new byte[] { 0x0});
                // Start the server
                server = new SimpleTcpServer().Start(8910);
                // Set \n as the delimited
                server.Delimiter = 0x13;
                server.ClientConnected += (sender, client) =>
                {
                    Console.WriteLine("Connected");
                };
                
                server.DelimiterDataReceived += (sender, msg) =>
                {
                    try
                    {
                        Console.WriteLine($"Msg: {msg.MessageString}");

                        // Send one of the control codes

                        if (msg.MessageString == "END")
                            CleanExit();

                        // Messages are in the format of 1,2,3,4. These correspond to our Arduino code:
                        // 0, ignored
                        // 1 = Player 1 ON
                        // 2 = Player 1 OFF
                        // 3 = Player 2 ON
                        // 4 = Player 2 OFF
                        byte tempInt;
                        if (byte.TryParse(msg.MessageString, out tempInt))
                        {
                            serial.WriteLine(msg.MessageString + Environment.NewLine);
                            serial.DiscardOutBuffer();
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing command: " + ex);
                    }

                };

                // Add a quick tray menu to quit the server if it fails
                trayMenu = new ContextMenu();
                var mnuExit = new MenuItem("Exit");
                trayMenu.MenuItems.Add(0, mnuExit);

                notificationIcon = new NotifyIcon()
                {
                    Text = "Daytona Server",
                    Visible = true,
                    Icon = SystemIcons.Application,
                    ContextMenu = trayMenu
                };
                mnuExit.Click += (sender, eventArgs) => CleanExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                System.Windows.Forms.MessageBox.Show($"Error in server: {ex.ToString()}");
                CleanExit();
            }

            while (Console.ReadLine() == "")
            {
                // send 0's so the arduino knows we're connected
                serial.WriteLine("0");
                serial.BaseStream.Flush();
                serial.DiscardOutBuffer();
                Thread.Sleep(50);
            }
            
            CleanExit();
        }

        static void CleanExit()
        {
            Application.ExitThread();
            Application.Exit();
            Environment.Exit(0);

            try
            {
                if (server != null && server.IsStarted)
                    server.Stop();
                if (serial != null && serial.IsOpen)
                    serial.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error shutting down: " + ex);
            }
            
            
        }
    }
}
