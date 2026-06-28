using Biblioteca2;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new BibliotecaContext();

            //mostrar todos los libros ordenados por ISBN y luego por Titulo
            var libros = context.Libro
            .OrderBy(l => l.ISBN)
            .ThenBy(l => l.Titulo)
            .ToList();
            var prestamos = context.Prestamo
            .OrderBy(p => p.Socio)
            .ThenBy(p => p.Libro)
            .ToList();


            foreach (var l in libros)
            {
                Console.WriteLine($"{l.ISBN} | {l.Titulo} | {l.Autor} | {l.Genero} | {l.CantidadCopias}");
            }


            Console.WriteLine("Ingrese el numero de la accion que quiere realizar: ");
            Console.WriteLine("1. Libros más prestados");
            Console.WriteLine("2. Socios con multas pendientes");
            Console.WriteLine("3. Prestamos vencidos");
            Console.WriteLine("4. Disponibilidad de un libro");
            Console.WriteLine("5. Historial de un socio");

            int respuesta = Convert.ToInt32(Console.ReadLine());

            //Libros más prestados
            if (respuesta == 1) { }


            //Socios con multas pendientes 
            if (respuesta == 2) { }


            //Prestamos vencidos
            if (respuesta == 3) { //s es Socio
                if(s.FechaDevolucion < s.FechaVencimiento) //Conceptual, chequear para cada socio si la fecha de devolucion es menor a la fecha de vencimiento
                {

                } 

            }


            //Disponibilidad de un libro
            if (respuesta == 4) { }


            //Historial de un socio
            if (respuesta == 5) { }
        }
    }
}