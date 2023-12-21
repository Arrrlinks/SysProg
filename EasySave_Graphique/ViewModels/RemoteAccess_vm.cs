using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EasySave_Graphique.Models;

namespace EasySave_Graphique;

public static class RemoteAccess_vm
{
    private static RemoteAccess_m _remoteAccess = RemoteAccess_m.Instance;
    private static Socket _client;
    private static Socket _socket;

    static RemoteAccess_vm()
    {
        _remoteAccess._Reconnect = () => Reconnect();
    }
    

    public static void ServerConnection()
    {
        _socket = _remoteAccess.SeConnecter();
        _client = _remoteAccess.AccepterConnexion(_socket);
        _remoteAccess.EcouterRéseau(_client);
        _remoteAccess.Deconnecter(_socket);
    }

    private static void Reconnect()
    {
        _client = _remoteAccess.AccepterConnexion(_socket);
        _remoteAccess.EcouterRéseau(_client);
    }
    
}