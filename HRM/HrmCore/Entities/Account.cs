using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Account
    {
        public Guid guid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoleID { get; set; }

        public Guid StaffID { get; set; }
    }
}
