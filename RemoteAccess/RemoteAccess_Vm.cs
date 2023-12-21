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
        _model._displayError = DisplayErrror;
        _model._displayInfo = () => DisplayInfo(_model._input, _model._StringData);
        _model._displayDeconnexion = DisplayDeconnexion;
        _model._GetAction = () => _model._input = _vue.GetInput();
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
    
    public void DisplayInfo(string input, string data)
    {
        _vue.DisplayInfo(input, data);
    }

    public void DisplayErrror()
    {
        _vue.DisplayErrorConnexion();
    }
    
    public void DisplayDeconnexion()
    {
        _vue.DisplayDeconnexion();
    }
}