using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.ViewModels
{
    public class RoleCreateVM
    {
        public string Name { get; set; }
    }
    public class RoleEditVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
    public class RoleShowVM:RoleEditVM
    {

    }
}


