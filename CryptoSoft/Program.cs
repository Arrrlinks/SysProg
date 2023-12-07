// See https://aka.ms/new-console-template for more information

using System.Text;

namespace CryptoSoft
{
    class Program
    {
        static void Main(string[] args)
        {
            string data;
            //if not the good number of arguments, quit
            if (args.Length != 2)
            {
                throw new ArgumentNullException("Not enough arguments");
            }
            //if the key is not 8 bits long, quit
            if (args[1].Length != 8)
            {
                throw new ArgumentException("Byte must be 8 bits long");
            }
            //string to byte
            byte key = Convert.ToByte(args[1], 2);
            //check if the file exists
            if (File.Exists(args[0]))
            {
                //read the file
                data = File.ReadAllText(args[0]);
            }
            else
            {
                //if not, quit
                throw new FileNotFoundException("File not found");
            }
            //stringbuilder to store the binary string
            StringBuilder sb = new StringBuilder();
            
            //convert the string to binary
            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }

            //xor avec la clé qui est un byte
            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = (char)(int)(sb[i] ^ key);
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
