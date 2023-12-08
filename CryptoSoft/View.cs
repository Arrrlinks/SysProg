namespace CryptoSoft;

public class View
{
    public View(){}
    
    public string GetPath()
    {
        Console.WriteLine("Enter the path of the file you want to encrypt/decrypt");
        string path = Console.ReadLine();
        while (!File.Exists(path))
        {
            Console.WriteLine("Wrong path, enter the path of the file you want to encrypt/decrypt");
            path = Console.ReadLine();
        }
        return path;
    }
    
    public string GetKey()
    {
        Console.WriteLine("Enter the key you want to use");
        string key = Console.ReadLine();
        while (key.Length > 64 || key.Length == 0)
        {
            Console.WriteLine("Wrong key, enter the key you want to use");
            key = Console.ReadLine();
        }
        return key;
    }
}