

namespace Logging_Serilog.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Experience { get; set; } = null!;
    }
}
