using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EasySave_Graphique.ViewModels;

namespace EasySave_Graphique.Models;

public sealed class RemoteAccess_m
{
    
    private static IPEndPoint clientEndPoint;

    public Action _Reconnect;

    private ObservableCollection<backup_m> BackupMs;
    private state_m _state;
    private Save_vm _save;
    private bool _finish;

    private RemoteAccess_m()
    {
        _state = new state_m();
        _save = new Save_vm();
        BackupMs = new ObservableCollection<backup_m>();
        _finish = false;
    }
    
    public void SetFinish()
    {
        _finish = true;
    }

    
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
    
    //créer un socket serveur et l'attacher à une adresse ip et un port (celui du serveur)
    public Socket SeConnecter()
    {
        //créer un socket
        Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
        //port et ip
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("0.0.0.0"),9050 );
        //attibut port et ip
        socket.Bind(ipep);
           
        //écoute les connexions entrantes
        socket.Listen(10);
           
        return socket;
    }
    
    public Socket AccepterConnexion(Socket socket)
    {
        //Récup client socket
        Socket client = socket.Accept();
        //Récup client ip et port
        clientEndPoint = (IPEndPoint)client.RemoteEndPoint;
        return client;
    }
    
    //ferme le socket
    public void Deconnecter(Socket socket)
    {
    socket.Close();
    return;
    }

    public string ListToString(ObservableCollection<backup_m> backups)
    {
        StringBuilder listBuilder = new StringBuilder();

        for (int i = 0; i < backups.Count; i++)
        {
            listBuilder.AppendLine($"{i} : {backups[i].Name} - {backups[i].State} - {backups[i].Size} MB - {backups[i].Source} - {backups[i].Target}");
        }

        return listBuilder.ToString();
    }
    
    public void EcouterRéseau(Socket socket)
    {
    //encode et envoi bienvenu au client
            string imput;
            int recv;
            string welcome = "Bienvenue...\n";

            byte[] data = new byte[1024];

            data = Encoding.UTF8.GetBytes(welcome);
            socket.Send(data, data.Length, SocketFlags.None);
            
            string ListData;
            
            BackupMs = _state.GetBackupsFromStateFile();
            ListData = ListToString(BackupMs);
            socket.Send(Encoding.UTF8.GetBytes(ListData));

            while (true)
            {
                if (_finish == true)
                {
                    return;
                }
                BackupMs = _state.GetBackupsFromStateFile();
                ListData = ListToString(BackupMs);
                socket.Send(Encoding.UTF8.GetBytes(ListData));

                recv = socket.Receive(data);
                imput = Encoding.UTF8.GetString(data, 0, recv);
                
                
                //parse le message du client
                var msg = imput.Split(" ");
                if (msg.Length != 2 && msg[0] != "exit" || msg.Length == 2 && int.TryParse(msg[1], out _) == false)
                {
                }
                else
                {
                    switch (msg[0])
                    {
                        case "save":
                            _save.StartSave(BackupMs[Int32.Parse(msg[1])]);
                            //_save.SaveLaunch(BackupMs[Int32.Parse(msg[1])].Source, BackupMs[Int32.Parse(msg[1])].Target, BackupMs[Int32.Parse(msg[1])].Name, BackupMs[Int32.Parse(msg[1])]);
                            break;
                        
                        case "pause":
                            _save.StopSave(BackupMs[Int32.Parse(msg[1])]);
                            break;
                        
                        case "stop":
                            _save.StopSave(BackupMs[Int32.Parse(msg[1])]);
                            break;
                        
                        case "exit":
                            _Reconnect();
                            break;
                        
                        default: 
                            socket.Send(Encoding.UTF8.GetBytes("Commande non reconnue"));
                            break;
                    }
                }
            }
        }
}