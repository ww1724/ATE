using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Models
{
    public class User : BindableBase
    {
        public string  Name { get; set; }

        public string NickName { get; set; }

        public char Avatar { get; set; }

        public string Role { get; set; }
    }

    public class LoginUser : BindableBase
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
