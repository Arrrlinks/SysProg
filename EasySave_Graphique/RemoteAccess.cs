using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EasySave_Graphique.Models;

namespace EasySave_Graphique;

public static class RemoteAccess
{
    private static RemoteAccess_m _remoteAccess = RemoteAccess_m.Instance;

    public static void ServerConnection()
    {
        Socket socket = _remoteAccess.SeConnecter();
        Socket client = _remoteAccess.AccepterConnexion(socket);
        _remoteAccess.EcouterRéseau(client);
        _remoteAccess.Deconnecter(socket);
    }
    
}