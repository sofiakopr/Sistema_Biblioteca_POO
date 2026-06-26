using Biblioteca;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Biblioteca
{
    public class BibliotecaContext : DbContext
    {
        public DbSet<Libro> Libro { get; set; }
        public DbSet<Socio> Socio { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Prestamo> Prestamo { get; set; }
        public DbSet<EstadoPrestamo> EstadoPrestamo { get; set; }
        public DbSet<EstadoReserva> EstadoReserva { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=C:\Users\sofia\source\repos\Sistema_Biblioteca_POO-main\Biblioteca_Koprcina_Isern\Sistema_Biblioteca.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>()
                .ToTable("Libro")
                .HasKey(l => l.ISBN);
            modelBuilder.Entity<Libro>()
                .Property(l => l.ISBN).HasColumnName("ISBN");

            modelBuilder.Entity<Socio>()
                .ToTable("Socio")
                .HasKey(s => s.NumeroSocio);
            modelBuilder.Entity<Socio>()
                .Property(s => s.NumeroSocio).HasColumnName("NumeroSocio");

            modelBuilder.Entity<Reserva>()
                .ToTable("Reserva")
                .HasKey(r => new { r.Socio, r.Libro });
            modelBuilder.Entity<Reserva>()
                .Property(r => r.FechaReserva).HasColumnName("FechaReserva");
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Socio)
                .WithMany()
                .HasForeignKey(r => r.NumeroSocio);
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Libro)
                .WithMany()
                .HasForeignKey(r => r.ISBN);
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Estado)
                .WithMany()
                .HasForeignKey(r => r.EstadoReserva);

            modelBuilder.Entity<Prestamo>()
                .ToTable("Prestamo")
                .HasKey(d => new { d.Matricula, d.IdEspecialidad, d.DiaSemana });
            modelBuilder.Entity<Disponibilidad>()
                .Property(d => d.IdEspecialidad).HasColumnName("id_especialidad");
            modelBuilder.Entity<Disponibilidad>()
                .Property(d => d.DiaSemana).HasColumnName("dia_semana");

            modelBuilder.Entity<EstadoPrestamo>()
                .ToTable("EstadoPrestamo")
                .HasKey(ep => ep.Id);
            modelBuilder.Entity<EstadoPrestamo>()
                .Property(ep => ep.Id).HasColumnName("Id");
            modelBuilder.Entity<EstadoPrestamo>()
                .Property(ep => ep.Estado).HasColumnName("Estado");

            modelBuilder.Entity<EstadoReserva>()
                .ToTable("EstadoReserva")
                .HasKey(er => er.Id);
            modelBuilder.Entity<EstadoReserva>()
                .Property(er => er.Id).HasColumnName("Id");
            modelBuilder.Entity<EstadoReserva>()
                .Property(er => er.Estado).HasColumnName("Estado");

        }
    }
}