using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca
{

    public class Prestamo
    {
        public int Socio { get; set; }
        public string Libro { get; set; }
        public string FechaPrestamo { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaDevolucion { get; set; }
        public int Estado { get; set; }
    }
}

