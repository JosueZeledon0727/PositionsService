using Microsoft.EntityFrameworkCore;
using PositionsService.Models;

namespace PositionsService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        // DbSets
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionStatus> PositionStatuses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Data inicial para las tablas de referencia
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentID = 1, DepartmentName = "HR" },
                new Department { DepartmentID = 2, DepartmentName = "IT" },
                new Department { DepartmentID = 3, DepartmentName = "Sales" }
            );

            modelBuilder.Entity<PositionStatus>().HasData(
                new PositionStatus { PositionStatusID = 1, StatusName = "Active" },
                new PositionStatus { PositionStatusID = 2, StatusName = "Inactive" },
                new PositionStatus { PositionStatusID = 3, StatusName = "Closed" }
            );

            modelBuilder.Entity<Recruiter>().HasData(
                new Recruiter { RecruiterID = 1, RecruiterName = "Pancho Villa" },
                new Recruiter { RecruiterID = 2, RecruiterName = "Juan Ramirez" },
                new Recruiter { RecruiterID = 3, RecruiterName = "Federico Alvarez" }
            );


            // Definicion de Foreign Keys
            modelBuilder.Entity<Position>()
                .HasOne(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.PositionStatusID)
                .OnDelete(DeleteBehavior.Restrict);  // Restrict para que no se borren las positions en caso de eliminar dicha posicion, solo se pone en null

            modelBuilder.Entity<Position>()
                .HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.Recruiter)
                .WithMany()
                .HasForeignKey(p => p.RecruiterID)
                .OnDelete(DeleteBehavior.Restrict);

            // PositionNumber unico
            modelBuilder.Entity<Position>()
                .HasIndex(p => p.PositionNumber)
                .IsUnique();

            modelBuilder.Entity<Position>()
            .Property(p => p.Budget)
            .HasColumnType("decimal(18,2)")  // Garantiza que sea un decimal con 2 decimales
            .HasDefaultValue(0);  // Valor defecto a 0 por si acaso

            modelBuilder.Entity<Position>()
            .ToTable(table => table
                .HasCheckConstraint("CK_Position_Budget", "[Budget] >= 0"));
        }
    }
}
