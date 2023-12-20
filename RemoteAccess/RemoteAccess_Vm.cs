using System.Net.Sockets;

namespace Client;

public class RemoteAccess_Vm
{
    private RemoteAccess_v _vue;
    
    private static RemoteAccess_m _model = RemoteAccess_m.Instance;
    
    
    public RemoteAccess_Vm()
    {
        _vue = new RemoteAccess_v();
    }
    
    private void GetIp()
    {
        _model._ip = _vue.GetIp();
    }
    
    public void ServerConnexion()
    {
        GetIp();
        Socket socket = _model.Connexion();
        if (socket == null)
        {
            _vue.DisplayErrorConnexion();
            return;
        }
        _model.Listen(socket);
    }
}