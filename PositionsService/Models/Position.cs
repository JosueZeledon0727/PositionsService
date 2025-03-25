namespace PositionsService.Models
{
    public class Position
    {
        public int PositionID { get; set; }
        public required string PositionNumber { get; set; } // Tiene que ser unique (acordarme)
        public required string Title { get; set; }
        public int PositionStatusID { get; set; }
        public int DepartmentID { get; set; }
        public int RecruiterID { get; set; }
        public decimal Budget { get; set; }

        // Foreign Keys
        public PositionStatus Status { get; set; }
        public Department Department { get; set; }
        public Recruiter Recruiter { get; set; }
    }
}
