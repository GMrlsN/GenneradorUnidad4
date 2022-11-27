using System;

namespace Generador
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje a = new Lenguaje("c2.gram"))
                {
                    a.gramatica();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
