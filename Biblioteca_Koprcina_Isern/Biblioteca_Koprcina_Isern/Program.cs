using System.Security.Cryptography.X509Certificates;
using Biblioteca2;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new BibliotecaContext();


            int bucle = 1;

            while (bucle > 0)
            {
                //mostrar todos los libros ordenados por ISBN y luego por Titulo
                var libros = context.Libro
                .OrderBy(l => l.ISBN)
                .ThenBy(l => l.Titulo)
                .ToList();


                foreach (var l in libros)
                {
                    Console.WriteLine($"{l.ISBN} | {l.Titulo} | {l.Autor} | {l.Genero} | {l.CantidadCopias}");
                }

                Console.WriteLine("Ingrese el numero de la accion que quiere realizar: ");
                Console.WriteLine("1. Flujo de préstamo");
                Console.WriteLine("2. Flujo de devolución");
                Console.WriteLine("3. Flujo de reserva");
                Console.WriteLine("4. Mostrar detalles de un Socio");

                int respuesta = Convert.ToInt32(Console.ReadLine());

                //Libros más prestados
                if (respuesta == 1)
                {
                    FlujoPrestamo(context);
                }


                //Flujo Devolución
                if (respuesta == 2)
                {
                    FlujoDevolucion(context);
                }


                //Flujo Reserva
                if (respuesta == 3)
                {
                    FlujoReserva(context);
                }


                //Detalle de un Socio
                if (respuesta == 4)
                {
                    DetallesSocio(context);
                }
                
                Console.Clear();
                Console.WriteLine("Desea Continuar?");
                Console.WriteLine("0. No");
                Console.WriteLine("1. Si");
                bucle = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
        }

        static void DetallesSocio(BibliotecaContext context)
        {
            Console.WriteLine("Ingrese el ID de usuario");
            int socio_id = Convert.ToInt32(Console.ReadLine());

            var socio = context.Socio
                .Include(s => s.Tipo) // Es clave para poder usar socio.Tipo después
                .FirstOrDefault(s => s.NumeroSocio == socio_id);

            if (socio == null)
            {
                Console.WriteLine("Ese Socio no existe");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            if (socio.Activo == 0)
            {
                Console.WriteLine($"ID: {socio.NumeroSocio} | Nombre Completo {socio.Nombre} {socio.Apellido}");
                Console.WriteLine($"Mail: {socio.Email} | Tipo: {socio.Tipo.Tipo} | Activo: No");
            }
            else
            {
                Console.WriteLine($"ID: {socio.NumeroSocio} | Nombre Completo {socio.Nombre} {socio.Apellido}");
                Console.WriteLine($"Mail: {socio.Email} | Tipo: {socio.Tipo.Tipo} | Activo: Si");
            }

            var reservas = context.Reserva
                .Where(r => r.NumeroSocio == socio_id)
                .Include(r => r.Libro)
                .Include(r => r.Estado)
                .OrderBy(r => r.ISBN)
                .ToList();

            Console.WriteLine("Reservas:");

            foreach (var r in reservas)
            {
                Console.WriteLine($" ISBN {r.ISBN} | Titulo {r.Libro.Titulo} | Estado {r.Estado.Estado} | Fecha {r.FechaReserva}");
            }

            Console.WriteLine("Prestamos:");

            var prestamos = context.Prestamo
                .Where(p => p.NumeroSocio == socio_id)
                .Include(p => p.Libro)
                .Include(p => p.Estado)
                .OrderBy(p => p.ISBN)
                .ToList();

            foreach (var p in prestamos)
            {
                Console.WriteLine($" ISBN {p.ISBN} | Titulo {p.Libro.Titulo} | Estado {p.Estado.Estado} | Fecha Vencimiento {p.FechaVencimiento}");
            }



            Console.WriteLine("Pulse Cualquier Tecla Para Continuar");
            Console.ReadLine();

        }

    static void FlujoReserva(BibliotecaContext context)
        {

            Console.WriteLine("Que acción quiere hacer?");
            Console.WriteLine("1. Hacer una reserva");
            Console.WriteLine("2. Consultar una reserva");
            Console.WriteLine("3. Cancelar una reserva");
            int eleccion = Convert.ToInt32(Console.ReadLine());
            
            Console.WriteLine("Ingrese el id del Socio");
            int socio_id = Convert.ToInt32(Console.ReadLine());

            var socio = context.Socio
                .Include(s => s.Tipo) // Es clave para poder usar socio.Tipo después
                .FirstOrDefault(s => s.NumeroSocio == socio_id);

            if (socio == null)
            {
                Console.WriteLine("Ese Socio no existe");
                System.Threading.Thread.Sleep(1000);
                return;
            }


            Console.WriteLine("Ingrese el ISBN del Libro");
            string ISBNreserva = Console.ReadLine();
            var libro = context.Libro
                .FirstOrDefault(l => l.ISBN == ISBNreserva);

            if (libro == null)
            {
                Console.WriteLine("Ese libro no está");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            switch (eleccion)
            {
                case 1:

                    var buscarReserva = context.Reserva.FirstOrDefault(r => r.NumeroSocio == socio.NumeroSocio && r.ISBN == libro.ISBN && r.EstadoId == 1);

                    if (buscarReserva == null)
                    {
                        var nuevaReserva = new Reserva
                        {
                            NumeroSocio = socio.NumeroSocio,
                            ISBN = libro.ISBN,
                            FechaReserva = DateTime.Now.ToString("yyyy-MM-dd"),
                            EstadoId = 1
                        };
                        Console.WriteLine("Reserva añadida con exito!");
                        context.Reserva.Add(nuevaReserva);
                    } else
                    {
                        Console.WriteLine("Esa reserva ya existe");
                    }
                    System.Threading.Thread.Sleep(1000);
                    break;

                case 2:

                    var reserva = context.Reserva.FirstOrDefault(r => r.NumeroSocio == socio.NumeroSocio && r.ISBN == libro.ISBN && r.EstadoId == 1);

                    if (reserva == null)
                    {
                        Console.WriteLine("No se econtro la reserva");
                        System.Threading.Thread.Sleep (1000);
                        break;
                    }
                    if (libro.CantidadCopias > 0)
                    {
                        Console.WriteLine("Ya hay copias para ese libro disponibles");



                        var nuevoPrestamo = new Prestamo
                        {
                            NumeroSocio = socio.NumeroSocio,
                            ISBN = libro.ISBN,
                            FechaPrestamo = DateTime.Now.ToString("yyyy-MM-dd"),
                            FechaVencimiento = DateTime.Now.AddDays(socio.Tipo.DiasPrestamo).ToString("yyyy-MM-dd"),
                            FechaDevolucion = null,
                            EstadoId = 1
                        };

                        context.Prestamo.Add(nuevoPrestamo);
                        libro.CantidadCopias--;
                    }
                    else
                    {
                        Console.WriteLine("Todavia no hay copias de ese libro disponibles");
                        System.Threading.Thread.Sleep(2000);
                    }
                    break;

                case 3:

                    var reservaa = context.Reserva.FirstOrDefault(r => r.NumeroSocio == socio.NumeroSocio && r.ISBN == libro.ISBN && r.EstadoId == 1);

                    if (reservaa == null)
                    {
                        Console.WriteLine("No se encontro la Reserva");
                    } else
                    {
                        Console.WriteLine("Reserva Cancelada Exitosamente");
                        reservaa.EstadoId = 3;
                    }
                    System.Threading.Thread.Sleep(2000);
                    break;
            }

             context.SaveChanges();

        }

    static void FlujoDevolucion(BibliotecaContext context)
        {
            Console.WriteLine("Ingrese id del Socio");
            int id_socio = Convert.ToInt32(Console.ReadLine());

            // 1. Buscamos al socio e INCLUIMOS los datos de su TipoSocio
            var socio = context.Socio
                .Include(s => s.Tipo) // Es clave para poder usar socio.Tipo después
                .FirstOrDefault(s => s.NumeroSocio == id_socio);
            if (socio == null)
            {
                Console.WriteLine("El socio no existe.");
                System.Threading.Thread.Sleep(2000);
                return;
            }

            if (socio.Activo == 0)
            {
                Console.WriteLine("El socio se encuentra inactivo.");
                System.Threading.Thread.Sleep(2000);
                return;
            }

            var prestamosActivos = context.Prestamo
                .Include(p => p.Socio).ThenInclude(s => s.Tipo)
                .Include(p => p.Libro)
                .Where(p => p.NumeroSocio == id_socio && p.FechaDevolucion == null)
                .OrderBy(p => p.ISBN)
                .ToList();

            if (prestamosActivos.Count == 0)
            {
                Console.WriteLine("Este socio no tiene préstamos activos en este momento.");
                System.Threading.Thread.Sleep(2000);
                return;
            }

            string ISBMdevolver;
            bool ISBMcorrecto = false;
            while (!ISBMcorrecto) {
                Console.WriteLine("\nLibros pendientes de devolución:");
                foreach (var p in prestamosActivos)
                {
                    Console.WriteLine($"ISBN: {p.ISBN} | Título: {p.Libro.Titulo} | Prestado el: {p.FechaPrestamo} | Vence el: {p.FechaVencimiento}");
                }

                Console.WriteLine("Ingrese ISBM del Libro a devolver");
                ISBMdevolver = Console.ReadLine();

                var prestamoEncontrado = context.Prestamo
                    .Include(p => p.Socio).ThenInclude(s => s.Tipo)
                    .FirstOrDefault(p => p.ISBN == ISBMdevolver);

                if(prestamoEncontrado == null)
                {
                    Console.WriteLine("Ese ISBM no es de ninguno de los libros a devolver");
                    ISBMcorrecto = false;
                }
                else
                {
                    if (Convert.ToDateTime(prestamoEncontrado.FechaVencimiento) >= DateTime.Now)
                    {
                        Console.WriteLine("El libro fue devuelto correctamente, Muchas Gracias!");
                        prestamoEncontrado.Libro.CantidadCopias++;
                        prestamoEncontrado.FechaDevolucion = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        Console.WriteLine("El libro no fue devuelto correctamente");

                        int multa = prestamoEncontrado.Socio.Tipo.MultaXDia * Convert.ToInt32((DateTime.Now - Convert.ToDateTime(prestamoEncontrado.FechaDevolucion)).Days);
                        Console.WriteLine($" Se debera pagar una Suma de: {multa}");
                        prestamoEncontrado.Libro.CantidadCopias++;
                    }

                    prestamoEncontrado.EstadoId = 2;
                    context.SaveChanges();
                    ISBMcorrecto = true;
                    System.Threading.Thread.Sleep(2000);

                }
            }

        }

    static void FlujoPrestamo (BibliotecaContext context)
        {
            Console.WriteLine("Ingrese id del Socio:");
            int id_socio = Convert.ToInt32(Console.ReadLine());

            // 1. Buscamos al socio e INCLUIMOS los datos de su TipoSocio
            var socio = context.Socio
                .Include(s => s.Tipo) // Es clave para poder usar socio.Tipo después
                .FirstOrDefault(s => s.NumeroSocio == id_socio);

            if (socio == null)
            {
                Console.WriteLine("El socio no existe.");
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                // RN-01: Validar si está activo antes que nada
                if (socio.Activo == 0)
                {
                    Console.WriteLine("El socio se encuentra inactivo.");
                    System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    // 2. Contamos sus préstamos activos actuales
                    int cantidadPrestados = context.Prestamo.Count(p => p.NumeroSocio == id_socio && p.FechaDevolucion == null);

                    // 3. Comparamos contra el límite dinámico de SU tipo de socio (RN-04)
                    if (cantidadPrestados >= socio.Tipo.LibrosMax)
                    {
                        Console.WriteLine($"El socio ya superó el límite de libros simultáneos para su categoría ({socio.Tipo.Tipo}).");
                        Console.WriteLine($"Límite: {socio.Tipo.LibrosMax} | Actualmente prestados: {cantidadPrestados}");
                        System.Threading.Thread.Sleep(2000);
                    }
                    else
                    {
                        bool tenemos_libro = false;

                        while (!tenemos_libro)
                        {
                            Console.WriteLine("Socio habilitado para préstamo. Ingrese ISBN del libro a prestar");
                            string ISBM_buscar = Convert.ToString(Console.ReadLine());
                            var libroEncontrado = context.Libro
                                .FirstOrDefault(l => l.ISBN == ISBM_buscar);

                            if (libroEncontrado == null)
                            {
                                Console.WriteLine("Ese ISBM no pertenece a ningun Libro");
                            }
                            else
                            {
                                // 1. Validamos si el socio ya tiene este libro en su poder (sin fecha de devolución)
                                bool yaTieneElLibro = context.Prestamo.Any(p =>
                                    p.NumeroSocio == socio.NumeroSocio &&
                                    p.ISBN == libroEncontrado.ISBN &&
                                    p.FechaDevolucion == null);

                                if (yaTieneElLibro)
                                {
                                    Console.WriteLine("\n[ERROR] El socio ya tiene un préstamo activo de este libro y no lo ha devuelto.");
                                    Console.WriteLine("No se puede registrar el préstamo. Volviendo al menú...");
                                    System.Threading.Thread.Sleep(3000);
                                    break; // Sale del bucle para no intentar el .Add()
                                }
                                else
                                {
                                    // 2. Si no lo tiene activo, se crea el préstamo con el formato original "yyyy-MM-dd"
                                    var nuevoPrestamo = new Prestamo
                                    {
                                        NumeroSocio = socio.NumeroSocio,
                                        ISBN = libroEncontrado.ISBN,
                                        FechaPrestamo = DateTime.Now.ToString("yyyy-MM-dd"),
                                        FechaVencimiento = DateTime.Now.AddDays(socio.Tipo.DiasPrestamo).ToString("yyyy-MM-dd"),
                                        FechaDevolucion = null,
                                        EstadoId = 1
                                    };

                                    context.Prestamo.Add(nuevoPrestamo);
                                    libroEncontrado.CantidadCopias--;
                                    context.SaveChanges();

                                    Console.WriteLine("¡Préstamo registrado con éxito!");
                                    Console.WriteLine($"El libro debe devolverse antes del: {nuevoPrestamo.FechaVencimiento}");
                                    System.Threading.Thread.Sleep(3000);
                                    tenemos_libro = true;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}