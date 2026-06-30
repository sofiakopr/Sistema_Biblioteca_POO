using Biblioteca2;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Biblioteca2
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
            options.UseSqlite(@"Data Source=C:\Users\felip\Desktop\Sistema_Biblioteca_POO2\Biblioteca_Koprcina_Isern\Biblioteca_Koprcina_Isern\Sistema_Biblioteca.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Libro
            modelBuilder.Entity<Libro>()
                .ToTable("Libro")
                .HasKey(l => l.ISBN);

            // Socio
            modelBuilder.Entity<Socio>()
                .ToTable("Socio")
                .HasKey(s => s.NumeroSocio);
            modelBuilder.Entity<Socio>()
                .HasOne(s => s.Tipo)
                .WithMany()
                .HasForeignKey(s => s.TipoSocio);

            // Reserva
            modelBuilder.Entity<Reserva>()
                .ToTable("Reserva")
                .HasKey(r => new { r.NumeroSocio, r.ISBN });
            modelBuilder.Entity<Reserva>()
                .Property(r => r.FechaReserva).HasColumnName("FechaReserva");
            modelBuilder.Entity<Reserva>()
                .Property(r => r.ISBN).HasColumnName("Libro");
            modelBuilder.Entity<Reserva>()
                .Property(r => r.NumeroSocio).HasColumnName("Socio");
            modelBuilder.Entity<Reserva>()
                .Property(r => r.EstadoId).HasColumnName("Estado");

            // foreign keys for Reserva
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
                .HasForeignKey(r => r.EstadoId);

            // Prestamo
            modelBuilder.Entity<Prestamo>()
                .HasKey(p => new {p.NumeroSocio, p.ISBN, p.FechaPrestamo});
            modelBuilder.Entity<Prestamo>()
                .Property(p => p.ISBN).HasColumnName("Libro");
            modelBuilder.Entity<Prestamo>()
                .Property(p => p.EstadoId).HasColumnName("Estado");
            modelBuilder.Entity<Prestamo>()
                .Property(p => p.NumeroSocio).HasColumnName("Socio");
            modelBuilder.Entity<Prestamo>()
                .Property(p => p.FechaPrestamo).HasColumnName("FechaPrestamo");
            modelBuilder.Entity<Prestamo>()
                .Property(p => p.FechaVencimiento).HasColumnName("FechaVencimiento");
            modelBuilder.Entity<Prestamo>()
                .Property(p => p.FechaDevolucion).HasColumnName("FechaDevolucion");

            // foreign keys for Prestamo
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Socio)
                .WithMany()
                .HasForeignKey(p => p.NumeroSocio);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany()
                .HasForeignKey(p => p.ISBN);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Estado)
                .WithMany()
                .HasForeignKey(p => p.EstadoId);

            // EstadoPrestamo
            modelBuilder.Entity<EstadoPrestamo>()
                .ToTable("EstadoPrestamo")
                .HasKey(ep => ep.Id);
            modelBuilder.Entity<EstadoPrestamo>()
                .Property(ep => ep.Estado).HasColumnName("Estado");

            // EstadoReserva
            modelBuilder.Entity<EstadoReserva>()
                .ToTable("EstadoReserva")
                .HasKey(er => er.Id);
            modelBuilder.Entity<EstadoReserva>()
                .Property(er => er.Estado).HasColumnName("Estado");
        }
    }
}
