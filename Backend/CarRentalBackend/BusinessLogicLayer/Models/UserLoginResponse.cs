using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class UserLoginResponse
    {
        [Key]
        public string Token { get; set; }
        public bool UserExists { get; set; }
        // Add other properties as needed
    }
}
