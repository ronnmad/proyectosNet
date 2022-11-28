using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace random
{
    class Program
    {
        static void Main(string[] args)
        {
            //var seed = Environment.TickCount;
            var ini = DateTime.Now;
            Console.WriteLine("ON");
            var guid = Guid.NewGuid();
            var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 4));

            Random rnd = new Random(seed);
            int tope = 500000;
            var listanum = new Int32[tope];
            var lista = new string[tope];

            int indice = 0;
            var listareduce = new List<int>();
            var listanums = new int[tope];

            for (int i = 0; i < tope; i++)
            {
                listareduce.Add(i);
                listanums[i] = i;
            }

            for (int i= tope; i > 0; i--)
            {
                indice = rnd.Next(0, listareduce.Count );
                int num = listareduce[indice];
                lista[i - listareduce.Count] = (num.ToString());
                listareduce.RemoveAt(indice);
            }
            Console.WriteLine(ini+ "|"+ DateTime.Now );
            File.WriteAllLines("../../../archivos/random.txt", lista );
        }
    }
}
