using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Skapa ett TCPListner-objekt, börja lyssna och vänta på anslutning
            IPAddress myIp = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(myIp, 8001);
            tcpListener.Start();
            Console.WriteLine("Väntar på anslutning...");

            //Någon försöker ansluta. Acceptera anslutningen
            Socket socket = tcpListener.AcceptSocket();
            Console.WriteLine("Anslutnng accepterad från " + socket.RemoteEndPoint);

            //Tag emot meddelanden
            byte[] bMessages = new Byte[256];
            int messagSize = socket.Receive(bMessages);
            Console.WriteLine("Meddelandn mottogs");

            //Konvertera meddelanden till en string-variabel och skriv ut 
            string message = "";
            for (int i = 0; i < messagSize; i++)
                message += Convert.ToChar(bMessages[1]);

            Console.WriteLine("Meddelande: " + message);

            //Stäng anslutningen mot just den här klienten:
            socket.Close();

            //Sluta lyssna efter trafik:
            tcpListener.Stop();
        }
    }
}
