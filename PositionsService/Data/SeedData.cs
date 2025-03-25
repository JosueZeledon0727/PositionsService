using PositionsService.Models;

namespace PositionsService.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, DataContext context)
        {
            // Asegúrate de que la base de datos esté creada
            context.Database.EnsureCreated();

            // Verifica si ya existen datos en las tablas, si no, agrega datos de referencia
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { DepartmentID = 1, DepartmentName = "HR" },
                    new Department { DepartmentID = 2, DepartmentName = "IT" },
                    new Department { DepartmentID = 3, DepartmentName = "Sales" }
                );
                context.SaveChanges();
            }

            if (!context.PositionStatuses.Any())
            {
                context.PositionStatuses.AddRange(
                    new PositionStatus { PositionStatusID = 1, StatusName = "Active" },
                    new PositionStatus { PositionStatusID = 2, StatusName = "Inactive" },
                    new PositionStatus { PositionStatusID = 3, StatusName = "Closed" }
                );
                context.SaveChanges();
            }

            if (!context.Recruiters.Any())
            {
                context.Recruiters.AddRange(
                    new Recruiter { RecruiterID = 1, RecruiterName = "Pancho Villa" },
                    new Recruiter { RecruiterID = 2, RecruiterName = "Juan Ramirez" },
                    new Recruiter { RecruiterID = 3, RecruiterName = "Federico Alvarez" }
                );
                context.SaveChanges();
            }

            // Verifica si ya existen datos en la tabla Position, si no, agrega algunos registros de ejemplo
            if (!context.Positions.Any())
            {
                context.Positions.AddRange(
                    new Position
                    {
                        PositionNumber = "P001",
                        Title = "Software Engineer",
                        PositionStatusID = 1,  // "Active"
                        DepartmentID = 2,  // "IT"
                        RecruiterID = 2,  // "Juan Ramirez"
                        Budget = 50000
                    },
                    new Position
                    {
                        PositionNumber = "P002",
                        Title = "HR Manager",
                        PositionStatusID = 1,  // "Active"
                        DepartmentID = 1,  // "HR"
                        RecruiterID = 1,  // "Pancho Villa"
                        Budget = 60000
                    },
                    new Position
                    {
                        PositionNumber = "P003",
                        Title = "Sales Executive",
                        PositionStatusID = 2,  // "Inactive"
                        DepartmentID = 3,  // "Sales"
                        RecruiterID = 3,  // "Federico Alvarez"
                        Budget = 40000
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
