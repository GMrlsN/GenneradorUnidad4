//Gabriel Morales Nuñez
using System;
using System.IO;

namespace Generador
{
    public class Error : Exception
    {
        public Error(string mensaje, StreamWriter log) : base(mensaje)
        {
            Console.WriteLine(mensaje);
            log.WriteLine(mensaje);
        }
    }
}