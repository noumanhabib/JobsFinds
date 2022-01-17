using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobsFinds.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? CompanyId { get; set; }

        public DateTime PostingDate { get; set; } = DateTime.Now;
        public DateTime LastDate { get; set; }
        public string? Title { get; set; }
        public string? Position { get; set; }
        public string? SalaryRange { get; set; }
        public double? AverageYearlySalary { get; set; }
        public string? Location { get; set; }

        public string? JobNature { get; set; }

        public int? Vacancy { get; set; }

        public string? WorkCondition { get; set; }
        public string? WorkTime { get; set; }

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public string? ApplyLink { get; set; }
        public string? AdLink { get; set; }

    }
}
