using System.Text;

namespace CryptoSoft;

public class Model
{
    public Model(){}

    public void encrypt(string path, string key)
    {
        List<byte> bytes = new List<byte>();
        string text = "";
        int Index = 0;
        path = path.Replace("?"," ");
        //for each character in the file
        
        //test if the file contains some text
        foreach (byte character in File.ReadAllBytes(path))
        {
            bytes.Add((byte)(character^key[Index]));
            //Goes to 0 every time it reaches the end of the key
            Index = (Index+1) % key.Length;
        }
        //return the encrypted part of the file in the original state
        File.WriteAllBytes(path, bytes.ToArray());
    }
}