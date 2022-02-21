//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace TicketSystem.Models
//{
//    public class UserActProblem
//    {     
//        public int Id { get; set; }
//        public int ProblemId { get; set; }
//        public int UserId { get; set; }
//        [EnumDataType(typeof(UserAction))]
//        public UserAction UserAction { get; set; }
//        [ForeignKey("UserId")]
//        public virtual User Users { get; set; }
//        [ForeignKey("ProblemId")]
//        public virtual ICollection<Problem> Problems { get; set; }
        

//    }
//    public enum UserAction
//    {
//        Create=1,
//        Edit=2,
//        Resolved=3
//    }
//}
