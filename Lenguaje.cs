//Gabriel Morales Nuñez
using System;
using System.Collections.Generic;
//Requerimiento 1.-Construir un metodo para escribir en el archivo Lenguaje.cs identando el codigo     --Ya jala
//                 "{" incrementa un tabulador, "}" decrementa un tabulador
//Requerimiento 2.-Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la       --Ya jala
//                 primera produccion de la gramatica
//Requerimiento 3.-La primera produccion es publica y el resto es privada                              --Ya jala
//Requerimiento 4.-El costructor lexico parametrizado debe validar que la extension del archivo        --Ya jala
//                 a compilar sea .gen 
//                 si no es .gen debe lanzar una excepcion
//Requerimiento 5.-Resolver la ambiguedad de ST y SNT                                                  --Ya jala
//                 Recorrer linea por linea el archivo gram para extraer cada nombre de produccion     
//Requerimiento 6.-Agregar el parentesis izquierdo y el parentesis derecho escapados en la matriz
//                 de transiciones
//Requerimiento 7.-Implementar el Or y la cerradura epsilon (No va a haber ORs)
//                 
namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        int tabulador;
        string primeraProduccion = "";
        bool primeraVez;
        List <string> listaSNT;
        public Lenguaje(string nombre) : base(nombre)
        {
            listaSNT = new List<string>();
            tabulador = 0;
            primeraVez = true;
        }
        public Lenguaje()
        {
            listaSNT = new List<string>();
            tabulador = 0;
            primeraVez = true;
        }
        public void Dispose()
        {
            cerrar();
        }
        private bool esSNT(string contenido)
        {
            //return true;
            return listaSNT.Contains(contenido);
        }
        private void agregarSNT(string contenido)
        {
            //Requerimiento 5
            listaSNT.Add(contenido);
        }
        private void WriteLineTP(string contenido)
        {
            //Requerimiento 1
            if(contenido == "}")
            {
                tabulador--;
            }
            for (int i = 0; i < tabulador; i++)
            {
                programa.Write("    ");
            }
            if (contenido == "{")
            {
                tabulador++;
            }
            programa.WriteLine(contenido);
        }
        private void WriteLineTL(string contenido)
        {
            //Requerimiento 1
            if (contenido == "}")
            {
                tabulador--;
            }
            for (int i = 0; i < tabulador; i++)
            {
                lenguaje.Write("    ");
            }
            if (contenido == "{")
            {
                tabulador++;
            }
            lenguaje.WriteLine(contenido);
        }
        private void leerLista()
        {
            //string nombre = archivo.            
            //Requerimiento 5
            string linea = "";
            linea = archivoR.ReadLine();
            linea = archivoR.ReadLine();
            //Console.WriteLine(linea);
            while (linea != null)
            {
                //Console.WriteLine(linea);
                string[] partes = linea.Split('-');
                partes[0] = partes[0].Trim(' ');
                agregarSNT(partes[0]);
                //Console.WriteLine(partes[0]);
                linea = archivoR.ReadLine();
            }
            //archivoR.Close();
            //archivo = new System.IO.StreamReader("c2.gram");
            //archivo.DiscardBufferedData(); 
            //archivo.BaseStream.Position = 0;
            //archivo.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        }
        private void Programa( string produccionPrincipal)
        {   
            WriteLineTP("using System;");
            WriteLineTP("using System.IO;");
            WriteLineTP("using System.Collections.Generic;");
            WriteLineTP("");
            WriteLineTP("namespace Generico");
            WriteLineTP("{");
            WriteLineTP("public class Program");
            WriteLineTP("{");
            WriteLineTP("static void Main(string[] args)");
            WriteLineTP("{");
            WriteLineTP("try");
            WriteLineTP("{");
            WriteLineTP("using (Lenguaje a = new Lenguaje())");
            WriteLineTP("{");
            WriteLineTP("a." + produccionPrincipal + "();");
            WriteLineTP("}");
            WriteLineTP("}");
            WriteLineTP("catch (Exception e)");
            WriteLineTP("{");
            WriteLineTP("Console.WriteLine(e.Message);");
            WriteLineTP("}");
            WriteLineTP("}");
            WriteLineTP("}");
            WriteLineTP("}");
        }
        public void gramatica()
        {
            leerLista();
            cabecera();
            Programa(primeraProduccion);
            cabeceraLenguaje();
            listaProducciones();
            WriteLineTL("}");
            WriteLineTL("}");
        }
        private void cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.ST);
            match(Tipos.FinProduccion);
            //Requerimiento 2
            primeraProduccion = getContenido();
        }
        private void cabeceraLenguaje()
        {
            WriteLineTL("using System;");
            WriteLineTL("using System.Collections.Generic;");
            WriteLineTL("namespace Generico");
            WriteLineTL("{");
            WriteLineTL("public class Lenguaje : Sintaxis, IDisposable");
            WriteLineTL("{");
            WriteLineTL("public Lenguaje(string nombre) : base(nombre)");
            WriteLineTL("{");
            WriteLineTL("}");
            WriteLineTL("public Lenguaje()");
            WriteLineTL("{");
            WriteLineTL("}");
            WriteLineTL("public void Dispose()");
            WriteLineTL("{");
            WriteLineTL("cerrar();");
            WriteLineTL("}");
        }
        private void listaProducciones()
        {
            if(!primeraVez){
                WriteLineTL("private void "+getContenido()+"()");
            }
            else{
                //Requerimiento 3
                WriteLineTL("public void "+getContenido()+"()");
                primeraVez = false;
            }
            WriteLineTL("{");
            match(Tipos.ST);
            match(Tipos.Produce);
            simbolos();
            match(Tipos.FinProduccion);
            WriteLineTL("}");
            if (!FinArchivo())
            {
                listaProducciones();
            }
        }
        private void simbolos()
        {
            if(getContenido() == "(")
            {
                match("(");
                WriteLineTL("if()");
                WriteLineTL("{");
                simbolos();
                match(")");
                WriteLineTL("}");
            }
            else if(esTipo(getContenido()))
            {
                WriteLineTL("match(Tipos." + getContenido() + ");");
                match(Tipos.ST);
            }
            else if(esSNT(getContenido()))
            {
                WriteLineTL(getContenido() + "();");
                match(Tipos.ST);
            }
            else if(getClasificacion() == Tipos.ST)
            {
                WriteLineTL("match(\"" + getContenido() + "\");");
                match(Tipos.ST);
            }
            if(getClasificacion() != Tipos.FinProduccion && getContenido() != ")")
            {
                simbolos();
            }
        }
        private bool esTipo(string clasificacion)
        {
            switch (clasificacion)
            {
                case "Identificador":
                case "Numero":
                case "Caracter":
                case "Asignacion":
                case "Inicializacion":
                case "OperadorLogico":
                case "OperadorRelacional":
                case "Operador ternario":
                case "OperadorTermino":
                case "OperadorFactor":
                case "IncrementoTermino":
                case "IncrementoFactor":
                case "FinSentencia":
                case "Cadena":
                case "TipoDato":
                case "Zona":
                case "Ciclo":
                case "Condicion":
                return true;

            }
            return false;
        }
    }
}