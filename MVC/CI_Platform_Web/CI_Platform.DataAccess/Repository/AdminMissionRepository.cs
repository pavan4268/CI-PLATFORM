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
    public class AdminMissionRepository:IAdminMissionRepository
    {
        private readonly CiPlatformDbContext _db;


        public AdminMissionRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public List<AdminMissionDisplayVm> GetMissions()
        {
            List<AdminMissionDisplayVm> missionList = new List<AdminMissionDisplayVm>();
            List<Mission> missions = _db.Missions.ToList();
            foreach(Mission mission in missions)
            {
                AdminMissionDisplayVm vm = new AdminMissionDisplayVm();
                vm.EndDate = mission.EndDate?.ToString("dd-MM-yyyy");
                vm.MissionId = mission.MissionId;
                vm.StartDate = mission.StartDate?.ToString("dd-MM-yyyy");
                vm.Title = mission.Title;
                vm.MissionType = mission.MissionType;
                missionList.Add(vm);
            }
            return missionList;
        }


        public AdminMissionCreateVm FillDropDown()
        {
            AdminMissionCreateVm vm = new AdminMissionCreateVm();
            List<Country>? Countries = _db.Countries.Where(country=>country.DeletedAt==null).ToList();
            List<MissionTheme>? Themes  =_db.MissionThemes.Where(theme=>theme.Status==1 && theme.DeletedAt==null).ToList();
            List<Skill>? SkillList = _db.Skills.Where(skill=>skill.DeletedAt==null && skill.Status==1).ToList();
            vm.SkillList = SkillList;
            vm.Countries = Countries;
            vm.MissionThemes = Themes;
            return vm;
        }

        #region Mission Add
        public string AddMission(AdminMissionCreateVm obj)
        {
            string reply = string.Empty;
            if (obj != null)
            {
                Mission addmission = new Mission();
                addmission.CityId = obj.CityId;
                addmission.ThemeId = obj.ThemeId;
                addmission.CountryId = obj.CountryId;
                addmission.Title = obj.Title;
                addmission.ShortDescription = obj.ShortDescription;
                addmission.Description = obj.Description;
                addmission.StartDate = obj.StartDate;
                addmission.EndDate = obj.EndDate;
                addmission.MissionType = obj.MissionType;
                addmission.OrganizationDetails = obj.OrganizationDetails;
                addmission.OrganizationName = obj.OrganizationName;
                addmission.Availability = obj.Availability;
                
                if (obj.MissionType == "Time")
                {
                    addmission.TotalSeats = obj.TotalSeats;
                    
                }
                _db.Missions.Add(addmission);
                _db.SaveChanges(true);
                if(obj.SkillIds != null)
                {
                    List<MissionSkill> AddSkills = new List<MissionSkill>();
                    foreach (var skillId in obj.SkillIds)
                    {
                        MissionSkill AddSkill = new MissionSkill();
                        AddSkill.SkillId = skillId;
                        AddSkill.MissionId = addmission.MissionId;
                        AddSkills.Add(AddSkill);
                    }
                    _db.MissionSkills.AddRange(AddSkills);
                    _db.SaveChanges();
                }
                if (obj.MissionType == "Goal")
                {
                    GoalMission goaldata = new GoalMission();
                    goaldata.MissionId = addmission.MissionId;
                    goaldata.GoalObjectiveText = obj.GoalObjective;
                    goaldata.GoalValue = (int)obj.GoalValue;
                    _db.GoalMissions.Add(goaldata);
                    _db.SaveChanges();
                }
                if(obj.Documentpaths != null)
                {
                    foreach(string doc in obj.Documentpaths)
                    {
                        MissionDocument document = new MissionDocument();
                        document.MissionId = addmission.MissionId;
                        document.DocumentType = "doc";
                        document.DocumentPath = doc;
                        _db.MissionDocuments.Add(document);
                        _db.SaveChanges(true);
                    }
                }
                if(obj.Imagepaths != null)
                {
                    foreach(string img in obj.Imagepaths)
                    {
                        MissionMedium image = new MissionMedium();
                        image.MissionId = addmission.MissionId;
                        image.MediaType = "img";
                        image.MediaPath = img;
                        _db.MissionMedia.Add(image);
                        _db.SaveChanges(true);
                    }
                }
                if(obj.VideoURL!= null)
                {
                    MissionMedium video = new MissionMedium();
                    video.MissionId = addmission.MissionId;
                    video.MediaType = "mp4";
                    video.MediaPath = obj.VideoURL;
                    _db.MissionMedia.Add(video);
                    _db.SaveChanges(true);
                }

                
                return reply;
            }
            reply = "Data not Sent Successfully";
            return reply;
        }
        #endregion


        #region GetEditData
        public AdminMissionCreateVm GetData(long missionid)
        {
            AdminMissionCreateVm vm = new AdminMissionCreateVm();
            vm.MissionId = missionid;
            List<Country>? Countries = _db.Countries.Where(country => country.DeletedAt == null).ToList();
            List<MissionTheme>? Themes = _db.MissionThemes.Where(theme => theme.Status == 1 && theme.DeletedAt == null).ToList();
            List<Skill>? SkillList = _db.Skills.Where(skill => skill.DeletedAt == null && skill.Status == 1).ToList();
            vm.SkillList = SkillList;
            vm.Countries = Countries;
            vm.MissionThemes = Themes;
            Mission? getmission = _db.Missions.FirstOrDefault(mission=>mission.MissionId == missionid);
            if(getmission != null)
            {
                vm.Title = getmission.Title;
                vm.Description = getmission.Description;
                vm.ShortDescription = getmission.ShortDescription;
                vm.ThemeId = getmission.ThemeId;
                vm.CityId = getmission.CityId;
                vm.CountryId = getmission.CountryId;
                vm.Availability = getmission.Availability;
                vm.MissionType = getmission.MissionType;
                if (vm.MissionType == "Time")
                {
                    vm.TotalSeats = getmission.TotalSeats;
                }
                else if(vm.MissionType =="Goal")
                {
                    GoalMission? goaldetails = _db.GoalMissions.FirstOrDefault(goal=>goal.MissionId == missionid);
                    if(goaldetails != null)
                    {
                        vm.GoalObjective = goaldetails.GoalObjectiveText;
                        vm.GoalValue = goaldetails.GoalValue;
                    }
                    
                }
                List<MissionMedium> medias = _db.MissionMedia.Where(x => x.MissionId == missionid && x.DeletedAt==null).ToList();
                if(medias != null)
                {
                    List<string> images = new List<string>();
                    foreach(var media in medias.Where(x=>x.MediaType=="img"))
                    {
                        images.Add(media.MediaPath);
                    }
                    vm.Imagepaths = images;
                    MissionMedium videopath = _db.MissionMedia.FirstOrDefault(x => x.MissionId == missionid && x.MediaType=="mp4");
                    if(videopath != null)
                    {
                        vm.VideoURL = videopath.MediaPath;
                    }
                   
                }
                List<MissionDocument> missiondocs = _db.MissionDocuments.Where(x=>x.MissionId == missionid && x.DeletedAt==null).ToList();
                if(missiondocs != null)
                {
                    List<string>? docs = new List<string>();
                    foreach(MissionDocument document in missiondocs)
                    {
                        docs.Add(document.DocumentPath);
                    }
                    vm.Documentpaths = docs;
                }

                List<MissionSkill> missionskills = _db.MissionSkills.Where(x=>x.MissionId ==missionid && x.DeletedAt==null).ToList();
                if (missionskills != null)
                {
                    List<long>? skills = new List<long>();
                    foreach(MissionSkill skill in missionskills)
                    {
                        skills.Add(skill.SkillId);
                    }
                    vm.SkillIds = skills;
                }
                

                
            }
            return vm;
        }
        #endregion


        //public string EditMission(AdminMissionCreateVm obj)
        //{
        //    Mission? editmission = _db.Missions.FirstOrDefault(mission=>mission.MissionId == obj.MissionId);
        //    if (editmission != null)
        //    {
        //        editmission.ThemeId = obj.ThemeId;
        //        editmission.CityId = obj.CityId;
        //        editmission.CountryId = obj.CountryId;
        //        editmission.Title = obj.Title;
        //        editmission.Description = obj.Description;
        //        editmission.ShortDescription = obj.ShortDescription;
        //        editmission.StartDate = obj.StartDate;
        //        editmission.EndDate = obj.EndDate;
        //        editmission.MissionType = obj.MissionType;
        //        editmission.OrganizationDetails = obj.OrganizationDetails;
        //        editmission.OrganizationName = obj.OrganizationName;
        //        editmission.Availability = obj.Availability;
        //        editmission.UpdatedAt = DateTime.Now;
        //        if(obj.MissionType == "Time")
        //        {
        //            editmission.TotalSeats = obj.TotalSeats;
        //        }
        //        _db.Missions.Update(editmission);
        //        _db.SaveChanges();
        //        if (obj.MissionType == "Goal")
        //        {
        //            editmission.TotalSeats = null;
        //            editmission.UpdatedAt = DateTime.Now;
        //            _db.Missions.Update(editmission);
        //            _db.SaveChanges(true);
        //            GoalMission? goaledit  = _db.GoalMissions.FirstOrDefault(mission=>mission.MissionId == obj.MissionId);
        //            if (goaledit != null)
        //            {
        //                goaledit.GoalValue = (int)obj.GoalValue;
        //                goaledit.GoalObjectiveText = obj.GoalObjective;
        //                goaledit.UpdatedAt = DateTime.Now;
        //                _db.GoalMissions.Update(goaledit);
        //            }
        //            else
        //            {
        //                GoalMission newgoal = new GoalMission();
        //            }
        //        }
            
        //    }


        //}
    }
}
