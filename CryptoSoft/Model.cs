namespace CryptoSoft;

public class Model
{
    public Model(){}

    public void encrypt(string path, string key)
    {
        string text = "";
        int Index = 0;
        //for each character in the file
        foreach (char character in File.ReadAllText(path))
        {
            text += (char) (character^key[Index]);
            //Goes to 0 every time it reaches the end of the key
            Index = (Index+1) % key.Length;
        }
        //Writes the encrypted text in the file
        File.WriteAllText(path, text);
    }
}