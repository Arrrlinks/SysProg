using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

public sealed class RemoteAccess_m
{
    public string _ip { get; set; }
    
    private static readonly object _lock = new object();
    public static RemoteAccess_m _instance = null;
    
    //Créer un RemoteAccess_m si il n'existe pas et le retourne
    //Utilise une protection multithread
    public static RemoteAccess_m Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new RemoteAccess_m();
                }
                return _instance;
            }
        }
    }
    
//connexion
    public Socket Connexion()
        {
            //get ip
            Console.OutputEncoding = Encoding.UTF8;
            //create socket
            Socket server = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            //port and ip
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(_ip),9050 );
            
            try
            {
                //connect to server
                server.Connect(ipep);
            }
            catch (SocketException e)
            {
                //write error if server connection failed
                Console.WriteLine("Unable to connect to server.");
                Console.WriteLine(e.ToString());
                return null;
            }
            return server;
        }

//listen to server
    public void Listen(Socket server)
        {
            //recup server message
            byte[] data = new byte[1024];
            int recv = server.Receive(data);
            string imput, stringData;
            
            //print server message
            stringData = Encoding.UTF8.GetString(data, 0, recv);
            
            Console.WriteLine(stringData);

            while (true)
            {
                //if user input is exit, close the socket
                imput = Console.ReadLine();
                if (imput == "exit")
                {
                    server.Send(Encoding.UTF8.GetBytes(imput));
                    Deconexion(server);
                    break;
                }
                //send user input to server
                //receive server message
                Console.WriteLine("Client: " + imput);
                server.Send(Encoding.UTF8.GetBytes(imput));
                data = new byte[1024];
                recv = server.Receive(data);
                stringData = Encoding.UTF8.GetString(data, 0, recv);
                Console.Clear();
                Console.WriteLine(stringData);
            }
        }
        
        private static void Deconexion(Socket server)
        {
            Console.WriteLine("Disconnecting from server...");
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            Console.WriteLine("Disconnected from server.");
            return;
        }
}