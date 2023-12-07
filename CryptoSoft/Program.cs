// See https://aka.ms/new-console-template for more information

using System.Text;

namespace CryptoSoft
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //initialise variables
            string path = args[0];
            string key = args[1];;
            string text = "";
            //if not the good number of arguments, quit
            if (args.Length != 2)
            {
                throw new ArgumentNullException("Not enough arguments");
            }
            //if the key is more than 64 bytes or null, quit
            if (key.Length == 0 | key.Length >32)
            {
                throw new ArgumentException("Byte must be 8 bits long");
            }
            
            int Index = 0;
            if (File.Exists(path))
            {
                //for each character in the file
                foreach (char character in File.ReadAllText(path))
                {
                    text += (char) (character^key[Index]);
                    Index = (Index+1) % key.Length;
                }
            }
            else
            {
                //if not, quit
                throw new FileNotFoundException("File not found");
            }
            File.WriteAllText(path, text);
        }
    }
}
