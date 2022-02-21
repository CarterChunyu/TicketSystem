using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Severity
    {
        public Severity()
        {
            Problems = new HashSet<Problem>();
        }
        public int Id { get; set; }
        [Display(Name = "SeverityName")]
        public string Name { get; set; }
        public virtual ICollection<Problem> Problems { get; set; }
    }
}
