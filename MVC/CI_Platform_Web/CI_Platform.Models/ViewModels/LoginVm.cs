using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class LoginVm
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
