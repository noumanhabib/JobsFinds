using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobsFinds.Models
{
    public class CompanyResponse
    {
        [Key]
        public int Id { get; set; }

        public int JobApplicationId { get; set; }

        public DateTime InterviewTime { get; set; }
        public string? InterviewLocation { get; set; }
        [Column(TypeName = "text")]
        public string? Message;
    }
}
