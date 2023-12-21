using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            RemoteAccess_Vm _vm = new RemoteAccess_Vm();
            _vm.ServerConnexion();
        }
    }
}

