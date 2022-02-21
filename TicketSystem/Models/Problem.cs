using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Problem
    {
        //public Problem()
        //{
        //    UserActProblems = new HashSet<UserActProblem>();
        //}
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        // public string ImgUrl { get; set; }
        [DefaultValue(false)]
        public bool isSolved { get; set; }
        public int SeverityId { get; set; }
        public int PriorityId { get; set; }
        public int ProblemCategoryId { get; set; }
        [ForeignKey("SeverityId")]
        public virtual Severity Severity { get; set; }
        [ForeignKey("PriorityId")]
        public virtual Priority Priority { get; set; }
        [ForeignKey("ProblemCategoryId")]
        public virtual ProblemCategory ProblemCategory { get; set; }

        //public virtual ICollection<UserActProblem> UserActProblems { get; set; }

    }
}
