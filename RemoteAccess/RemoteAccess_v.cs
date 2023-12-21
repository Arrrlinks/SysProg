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
    
    public void DisplayInfo(string input, string data)
    {
        Console.Clear();
        Console.WriteLine($"Client: {input} \n");
        Console.WriteLine(data);
        Console.WriteLine("\n -Save: save [id] \n -Pause: pause [id] \n -Stop: stop [id] \n -Exit: exit");
    }
    
    public void DisplayDeconnexion()
    {
        Console.WriteLine("Server disconnected.");
    }
    
    public string GetInput()
    {
        string input = "";
        Console.WriteLine("Enter your message:");
        do
        {
            input = Console.ReadLine();
        } while (input == "");
        
        return input;
    }
}