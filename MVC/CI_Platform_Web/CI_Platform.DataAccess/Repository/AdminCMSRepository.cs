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
            IEnumerable<CmsPage> cmsPages = _db.CmsPages.Where(cms=>cms.DeletedAt==null).ToList();
            foreach (CmsPage cmsPage in cmsPages)
            {
                AdminCMSDisplayVm cms = new AdminCMSDisplayVm();
                cms.Status = cmsPage.Status;
                cms.Title = cmsPage.Title;
                cms.CmsPageId = cmsPage.CmsPageId;
                cmslist.Add(cms);
            }
            return cmslist;
        }



        public void AddCMS(AdminCMSCreateVm obj)
        {
            CmsPage cms = new CmsPage();
            cms.Title=obj.Title;
            cms.Description = obj.Description;
            cms.Slug = obj.Slug;
            cms.Status=obj.Status;
            _db.CmsPages.Add(cms);
            _db.SaveChanges();
        }


        public AdminCMSCreateVm GetCMSDetails(long cmspageid)
        {
            CmsPage? getcms = _db.CmsPages.Find(cmspageid);
            if(getcms != null)
            {
                AdminCMSCreateVm cms = new AdminCMSCreateVm();
                cms.CmsPageId = getcms.CmsPageId;
                cms.Title = getcms.Title;
                cms.Description = getcms.Description;
                cms.Slug = getcms.Slug;
                cms.Status=getcms.Status;
                return cms;
            }
            return null;
        }



        public string SaveEditData(AdminCMSCreateVm obj)
        {
            
            CmsPage? getpage = _db.CmsPages.Find(obj.CmsPageId);
            if(getpage != null)
            {
                getpage.Title = obj.Title;
                getpage.Description = obj.Description;
                getpage.Slug = obj.Slug;
                getpage.Status=obj.Status;
                getpage.UpdatedAt = DateTime.Now;
                _db.CmsPages.Update(getpage);
                _db.SaveChanges(true);
                string reply = "Data Updated Successfully.";
                return reply;
            }
            return null;
        }


        public string DeleteCMS(long cmspageid)
        {
            string reply = "";
            CmsPage? deletepage = _db.CmsPages.Find(cmspageid);
            if(deletepage != null)
            {
                deletepage.DeletedAt = DateTime.Now;
                _db.CmsPages.Update(deletepage);
                _db.SaveChanges();
                reply = "CMS Deleted sucessfully";
                return reply;
            }
            return reply;
        }
    }
}
