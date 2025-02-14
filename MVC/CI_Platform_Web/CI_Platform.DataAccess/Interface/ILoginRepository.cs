﻿using CI_Platform.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface ILoginRepository
    {
         Task<IEnumerable<User>> getuser();
        Task<User> AuthenticateUser(string email, string password);
    }
}
