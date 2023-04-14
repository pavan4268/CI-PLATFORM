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
    public class AdminCMSRepository: IAdminCMSRepository
    {
        private readonly CiPlatformDbContext _db;

        public AdminCMSRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public List<AdminCMSDisplayVm> GetCms()
        {
            List<AdminCMSDisplayVm> cmslist = new List<AdminCMSDisplayVm>();
            IEnumerable<CmsPage> cmsPages = _db.CmsPages.ToList();
            foreach (CmsPage cmsPage in cmsPages)
            {
                AdminCMSDisplayVm cms = new AdminCMSDisplayVm();
                cms.Status = cmsPage.Status;
                cms.Title = cmsPage.Title;
                cmslist.Add(cms);
            }
            return cmslist;
        }


    }
}
