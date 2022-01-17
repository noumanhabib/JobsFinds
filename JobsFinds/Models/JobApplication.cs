using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobsFinds.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }
        public int JobId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? CompanyId { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? StudentId { get; set; }

        [Column(TypeName = "text")]
        public string? AboutMe { get; set; }

        [Column(TypeName = "text")]
        public string? WorkExperience { get; set; }

        public string? EducationLevel { get; set; }
        public string? ResidenceCity { get; set; }

        public string? cv { get; set; }

    }
}
