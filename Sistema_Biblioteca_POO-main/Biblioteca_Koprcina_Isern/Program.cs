﻿using Biblioteca;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new BibliotecaContext();

            //seleccionar y cargar todos los turnos y sus relaciones
            var libros = context.Libro
                .Include(l => l.ISBN)
                .Include(l => l.Titulo)
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .Include(l => l.CantidadCopias)
                .OrderBy(l => l.ISBN)
                .ThenBy(l => l.Titulo)
                .ToList();

            foreach (var l in libros)
            {
                Console.WriteLine($"{l.ISBN} | {l.Titulo} {l.Autor} | {l.Genero} {l.CantidadCopias}");
            }
        }
    }
}