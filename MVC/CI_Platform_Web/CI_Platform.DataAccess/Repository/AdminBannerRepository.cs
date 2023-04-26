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
    public class AdminBannerRepository : IAdminBannerRepository
    {
        private readonly CiPlatformDbContext _db;


        public AdminBannerRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public List<AdminBannerDisplayVm> GetBanner()
        {
            List<AdminBannerDisplayVm> fillBanners = new List<AdminBannerDisplayVm>();
            List<Banner>? banners = _db.Banners.Where(banner=>banner.DeletedAt==null).ToList();
            if (banners.Any())
            {
                foreach(Banner banner in banners)
                {
                    AdminBannerDisplayVm bannerDisplay = new AdminBannerDisplayVm();
                    bannerDisplay.BannerId = banner.BannerId;
                    bannerDisplay.SortOrder = banner.SortOrder;
                    bannerDisplay.Text = banner.Text;
                    bannerDisplay.Image = banner.Image;
                    fillBanners.Add(bannerDisplay);
                }
                return fillBanners;
            }
            return fillBanners;
        }


        public string AddBanner(AdminBannerCreateVm obj)
        {
            string reply = string.Empty; 
            Banner addbanner = new Banner();
            addbanner.Text = obj.Text;
            addbanner.Image = obj.Image;
            addbanner.SortOrder = obj.SortOrder;
            _db.Banners.Add(addbanner);
            _db.SaveChanges();
            return reply;
        }

        public AdminBannerCreateVm GetEditData(long bannerid)
        {
            AdminBannerCreateVm getdetails = new AdminBannerCreateVm();
            Banner? getbanner = _db.Banners.FirstOrDefault(banner=>banner.BannerId == bannerid && banner.DeletedAt == null);
            if(getbanner != null)
            {
                getdetails.SortOrder = getbanner.SortOrder;
                getdetails.BannerId = getbanner.BannerId;
                getdetails.Text = getbanner.Text;
                getdetails.Image = getbanner.Image;
                return getdetails;
            }
            return getdetails;
        }

        public string SaveEditData(AdminBannerCreateVm obj)
        {
            string? reply = string.Empty;
            if(obj.BannerId != null)
            {
                Banner? getbanner = _db.Banners.FirstOrDefault(banner=>banner.BannerId==obj.BannerId && banner.DeletedAt == null);
                if(getbanner!= null)
                {
                    getbanner.SortOrder = obj.SortOrder;
                    getbanner.UpdatedAt = DateTime.Now;
                    getbanner.Text = obj.Text;
                    if(obj.Image != null)
                    {
                        getbanner.Image = obj.Image;
                    }
                    _db.Banners.Update(getbanner);
                    _db.SaveChanges();
                    return reply;
                }
                reply = "Banner Not Found";
                return reply;
            }
            reply = "BannerId Is Null";
            return reply;
        }



        public string DeleteBanner(long bannerid)
        {
            string reply = string.Empty;
            Banner? deletebanner = _db.Banners.FirstOrDefault(banner => banner.BannerId==bannerid && banner.DeletedAt == null);
            if(deletebanner!= null)
            {
                deletebanner.DeletedAt = DateTime.Now;
                _db.Banners.Update(deletebanner);
                _db.SaveChanges();
                return reply;
            }
            reply = "Banner Not Found";
            return reply;
        }


    }
}
