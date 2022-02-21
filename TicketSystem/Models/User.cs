using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        //public virtual ICollection<UserActProblem> UserActProblem { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
