using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

public sealed class RemoteAccess_m
{
    public string _ip { get; set; }
    public string _input { get; set; }
    public string _StringData { get; set; }
    
    //to use ViewModel Method in Model
    public Action _displayError;
    public Action _displayInfo;
    public Action _displayDeconnexion;
    public Action _GetAction;
    
    //singleton
    private static readonly object _lock = new object();
    public static RemoteAccess_m _instance = null;
    
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
                //Console.WriteLine("Unable to connect to server.");
                _displayError();
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
            
            //print server message
            _StringData = Encoding.UTF8.GetString(data, 0, recv);
            
            Console.WriteLine(_StringData);

            while (true)
            {
                //if user input is exit, close the socket
                _GetAction();
                if (_input == "exit")
                {
                    server.Send(Encoding.UTF8.GetBytes(_input));
                    Deconexion(server);
                    break;
                }
                //send user input to server
                //receive server message
                server.Send(Encoding.UTF8.GetBytes(_input));
                data = new byte[1024];
                Thread.Sleep(1000);
                recv = server.Receive(data);
                _StringData = Encoding.UTF8.GetString(data, 0, recv); 
                //print server message
                _displayInfo();
            }
        }
        
        private void Deconexion(Socket server)
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            _displayDeconnexion();
            return;
        }
}