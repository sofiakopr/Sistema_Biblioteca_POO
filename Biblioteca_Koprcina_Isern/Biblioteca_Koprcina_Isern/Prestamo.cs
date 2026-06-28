using System;
using System.Collections.Generic;
using System.Text;
using Biblioteca2;

namespace Biblioteca2
{

    public class Prestamo
    {
        // PK
        public int NumeroSocio { get; set; }
        public string ISBN { get; set; }
        public string FechaPrestamo { get; set; }
        public string FechaVencimiento { get; set; }
        public string? FechaDevolucion { get; set; }

        // FK
        public int EstadoId { get; set; }

        public Socio Socio { get; set; }
        public Libro Libro { get; set; }
        public EstadoPrestamo Estado { get; set; }
    }
}

