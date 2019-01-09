using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ServerTest
{
    //Fredrik Habib
    class Program
    {
        static TcpListener TcpListener;
        //Main(), lyssnar efter trafik. Loopar till dess att ctrl-c trycks. I varje varv
        //i loopen väntar servern på en ny anslutning.
        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);
       
            //Skapa ett TCPListner-objekt, börja lyssna och vänta på anslutning
            IPAddress myIp = IPAddress.Parse("127.0.0.1");
            TcpListener = new TcpListener(myIp, 8001);
            TcpListener.Start();
            while(true)
            {
                try
                {
                    Console.WriteLine("Väntar på anslutning...");

                    //Någon försöker ansluta. Acceptera anslutningen
                    Socket socket = TcpListener.AcceptSocket();
                    Console.WriteLine("Anslutnng accepterad från " + socket.RemoteEndPoint);

                    //Tag emot meddelanden
                    Byte[] bMessages = new Byte[256];
                    int messagSize = socket.Receive(bMessages);
                    Console.WriteLine("Meddelandé mottogs ...");
                    
                    //Konvertera meddelanden till en string-variabel och skriv ut 
                    string message = "";
                    for (int i = 0; i < messagSize; i++)
                        message += Convert.ToChar(bMessages[i]);
                    Console.WriteLine("Meddelande: " + message);

                    //Detta är en generisk lista som slumpar ut orden som har valts och som ska implimenteras när
                    //man kommunicerar med klienten 
                    Random wrong = new Random();
                    var list = new List<string> { "Tack", "Hejsan", "Goddag", "Nej du" };
                    int index = wrong.Next(list.Count);
                    var words = list[index];
                    list.RemoveAt(index);

                    Byte[] bSend = System.Text.Encoding.ASCII.GetBytes(words);
                    socket.Send(bSend);
                    Console.WriteLine("Svar skickat");

                    //Stäng anslutningen mot just den här klienten:
                    socket.Close();
                }
                //Om du inte är kopplat med servern så händer följande nedan
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }
        //CancelKeyPress(), anropas då användaren tycker in Ctrl-C.
        static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            //Sluta lyssna efter trafik:
            TcpListener.Stop();
            Console.WriteLine("Servern stängdes av!");
        }
    }
}
