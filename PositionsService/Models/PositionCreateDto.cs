namespace PositionsService.Models
{
    public class PositionCreateDto
    {
        public required string PositionNumber { get; set; }
        public required string Title { get; set; }
        public int PositionStatusID { get; set; }
        public int DepartmentID { get; set; }
        public int RecruiterID { get; set; }
        public decimal Budget { get; set; }
    }
}
