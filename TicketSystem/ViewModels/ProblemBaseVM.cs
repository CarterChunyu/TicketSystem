using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Models;

namespace TicketSystem.ViewModels
{

    public class ProblemBaseVM
    {
        [Required]
        public int ProblemCategoryId { get; set; }
        [Required]
        public string Description { get; set; }
        // public string ImgUrl { get; set; }
        [DefaultValue(false)]
        public bool isSolved { get; set; }
        [Required]
        public int SeverityId { get; set; }
        [Required]
        public int PriorityId { get; set; }
    }
    public class ProblemCreateVM : ProblemBaseVM
    {
        [Required]
        [Remote(action:"IsSummaryExisted",controller:"Problem")]
        public string Summary { get; set; }
    }
    public class ProblemEditVM : ProblemBaseVM
    {
        public int Id { get; set; }
        [Required]
        public string Summary { get; set; }
    }
    public class ProblemSolvedVM : ProblemEditVM
    {
        public virtual Severity Severity { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual ProblemCategory ProblemCategory { get; set; }
    }
    public class ProblemShowVM : ProblemSolvedVM
    {

    }

}
