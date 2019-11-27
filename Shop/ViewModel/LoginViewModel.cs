using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class LoginViewModel
    {
        
        [Required]
        [Display(Name ="User Name ")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display (Name ="Remember me")]

        public bool Rememberme { get; set; }
        }
}
