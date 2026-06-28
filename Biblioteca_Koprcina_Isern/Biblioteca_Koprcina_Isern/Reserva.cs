using System;
using System.Collections.Generic;
using System.Text;
using Biblioteca2;

namespace Biblioteca2
{

    public class Reserva
    {
        // PK + FK
        public int NumeroSocio { get; set; }
        public string ISBN { get; set; }

        // FK
        public int EstadoId { get; set; }

        public string FechaReserva { get; set; }

        
        public Socio Socio { get; set; }
        public Libro Libro { get; set; }
        public EstadoReserva Estado { get; set; }
    }
}

