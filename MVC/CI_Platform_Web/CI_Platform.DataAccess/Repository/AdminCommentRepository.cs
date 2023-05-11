using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform.Repository.Repository
{
    public class AdminCommentRepository
    {
        private readonly CiPlatformDbContext _db;

        public AdminCommentRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


    }
}
