using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
   public class User
    {
       public int UserId { get; set; }
       public string Role { get; set; }
       public string Username { get; set; }
       //public string Email { get; set; }
       public string Password { get; set; }
       public DateTime? InsertedDate { get; set; }
       public DateTime? UpdatedDate { get; set; }
    }
}
