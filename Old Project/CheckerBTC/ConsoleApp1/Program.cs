using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] list = {"ToaCoinTOA", "BitcoinBTC"};

            for (var i = 0; i < list.Length; i++)
            {
                for (int k = list[i].Length - 1; k >= 0; k--)
                    if (list[i].ToLower() == list[i])
                    {
                        list[i] = list[i].Substring(k + 1);
                        break;
                    }
            }
            foreach(var word in list)
            {
                Console.WriteLine(word);
            }
            Console.ReadKey();
        }
    }
}
