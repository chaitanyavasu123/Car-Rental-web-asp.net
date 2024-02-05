using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class User
    {
        [Key]
        public string email { get; set; }
        public string Password { get; set; }
        // Add other properties as needed
    }
}
