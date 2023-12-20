namespace Client;

public class RemoteAccess_v
{
    public RemoteAccess_v(){}
    public string GetIp()
    {
        Console.WriteLine("Enter server ip:");
        return Console.ReadLine();
    }

    public void DisplayErrorConnexion()
    {
        Console.WriteLine("Unable to connect to server.");
    }
}