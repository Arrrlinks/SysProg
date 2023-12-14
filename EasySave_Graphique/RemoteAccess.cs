using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySave_Graphique;

public class RemoteAccess
{
    private static IPEndPoint clientEndPoint;
        public void ServerPart()
        {
            Socket socket = SeConnecter();
            Socket client = AccepterConnexion(socket);
            EcouterRéseau(client);
            Deconnecter(socket);
        }
        
        
        //créer un socket serveur et l'attacher à une adresse ip et un port (celui du serveur)
        private static Socket SeConnecter()
        {
            Console.WriteLine("listen");
            //créer un socket
           Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
           //port et ip
           IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("0.0.0.0"),9050 );
           //attibut port et ip
           socket.Bind(ipep);
           
           Console.WriteLine("listening...");
           //écoute les connexions entrantes
           socket.Listen(10);
           
           return socket;
        }

        private static Socket AccepterConnexion(Socket socket)
        {
            //Récup client socket
            Socket client = socket.Accept();
            //Récup client ip et port
            clientEndPoint = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("connece");
            return client;
        }

        private static void EcouterRéseau(Socket socket)
        {
            //encode et envoi bienvenu au client
            string imput;
            int recv;
            string welcome = "Bienvenue...";

            byte[] data = new byte[1024];

            data = Encoding.UTF8.GetBytes(welcome);
            socket.Send(data, data.Length, SocketFlags.None);

            while (true)
            {
                recv = socket.Receive(data);

                //si le client envoi exit, on ferme le socket
                if (Encoding.UTF8.GetString(data, 0, recv) =="exit")
                {
                    break;
                }
                //envoi le message du serveur
                //socket.Send(Encoding.UTF8.GetBytes(imput));
            }
        }
        
        //ferme le socket
        private static void Deconnecter(Socket socket)
        {
            Console.WriteLine("Disconnect {0", clientEndPoint.Address);
            
            socket.Close();
            Console.ReadLine();
        }
        
    }