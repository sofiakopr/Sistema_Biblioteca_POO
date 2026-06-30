using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca2
{

    public class Libro
    {
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public int Prestamos { get; set; }
        public int CantidadCopias { get; set; }
    }
}

