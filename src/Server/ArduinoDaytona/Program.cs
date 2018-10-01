using GodSharp;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoDaytona
{
    class Program
    {
        static GodSerialPort serial;
        static SimpleTcpServer server;
        public static NotifyIcon notificationIcon;
        public static ContextMenu trayMenu = new ContextMenu();


        [STAThread]
        static void Main(string[] args)
        {

            // Note you won't see this app
            // The output type is windows app, so no console window will appear. 
            try
            {
                // Open the serial port
                serial = new GodSerialPort("COM3", 9600);

                serial.Open();

                // Start the server
                server = new SimpleTcpServer().Start(8910);
                // Set \n as the delimited
                server.Delimiter = 0x13;
                server.DelimiterDataReceived += (sender, msg) =>
                {
                    // Send one of the control codes

                    if (msg.MessageString == "END")
                        CleanExit();

                    // Messages are in the format of P1 / P2 / P3 / P4 etc
                    // We'll have the Arduino app just expect the same data
                    // That way if I ever add more devices they can just keep going
                    if (msg.MessageString.StartsWith("P") && msg.MessageString.Length == 2)
                        serial.WriteAsciiString(msg.MessageString);

                    // TODO: add support for VR buttons and lights for those with those. 
                };
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
                System.Windows.Forms.MessageBox.Show($"Error in server: {ex.ToString()}");
                CleanExit();
            }

            Application.Run();
            CleanExit();
        }

        static void CleanExit()
        {

            if (server != null && server.IsStarted)
                server.Stop();
            if (serial != null && serial.IsOpen)
                serial.Close();
            Environment.Exit(0);
        }
    }
}
