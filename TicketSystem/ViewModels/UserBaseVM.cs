using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Models;

namespace TicketSystem.ViewModels
{
    public class UserBaseVM
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Password)]

        public string Password { get; set; }
        public int RoleId { get; set; }
    }
    public class UserCreateVM:UserBaseVM
    {
        [Required]
        [Remote(action: "IsAccountExisted", controller: "User")]
        public string Account { get; set; }
    }
    public class UserEditVM : UserBaseVM
    {
        public int Id { get; set; }
        [Required]
        public string Account { get; set; }
    }
    public class UserShowVM:UserEditVM
    {
        public Role Role { get; set; }
    }
    public class UserLoginVM
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string DenialReason { get; set; }
    }
    public class UserPasswordVM
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Remote(action: "IsPasswordCorrect",controller: "User",AdditionalFields =nameof(Id))]
        public string CurrentPassword { get; set; }
        [Required] 
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ComparePassword { get; set; }
        public string ErrorMessage { get; set; }
    }
    // 登入狀態
    //public class LoginStateVM:UserEditVM
    //{
    //    public string RoleName { get; set; }
    //}

    //public class UserLoginVM
    //{
    //    [Required]
    //    public string Account { get; set; }
    //    [Required]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }
    //    public string DenialReason { get; set; }
    //}

}
