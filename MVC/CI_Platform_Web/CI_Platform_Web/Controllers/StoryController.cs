﻿using Microsoft.AspNetCore.Mvc;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System.Net.Mail;

namespace CI_Platform_Web.Controllers
{
    public class StoryController : Controller
    {

        private readonly CiPlatformDbContext _db;
        private readonly IStoryCardsRepository _storyCards;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly INotificationRepository _notification;

        public StoryController(CiPlatformDbContext db, IStoryCardsRepository storyCards, IWebHostEnvironment hostEnvironment, INotificationRepository notification)
        {
            _db = db;
            _storyCards = storyCards;
            _hostEnvironment = hostEnvironment;
            _notification = notification;
        }



        public IActionResult StoriesListing()
        {
            if (HttpContext.Session.GetString("FirstName") != null)
            {
                var storylist = _storyCards.GetStories();
                return View(storylist);
            }
            return RedirectToAction("Index", "Home");


        }

        public IActionResult ShareYourStory()
        {

            if (HttpContext.Session.GetString("FirstName") != null)
            {
                string? user = HttpContext.Session.GetString("UserId");
                long userid = long.Parse(user);
                var usermissions = _storyCards.GetUserMissions(userid);
                return View(usermissions);
            }
            return RedirectToAction("Index", "Home");


            
        }

        


        public IActionResult StoryDetails(long storyid)
        {
            if(HttpContext.Session.GetString("UserId") == null)
            {
                TempData["storyid"] = (Int32)storyid;
                return RedirectToAction("Index", "Home");
            }
            var story = _storyCards.GetStoryDetails(storyid);
            return View(story);
        }








        [HttpPost]
        public bool StoryDetails(long storyid, List<string> selecteduser)
        {
            string? user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var currentuser = _db.Users.FirstOrDefault(x=> x.UserId == userid);
            var usertomail = _db.Users.Where(x=>selecteduser.Contains(x.UserId.ToString())).ToList();
            if (usertomail != null)
            {
                foreach(var users in usertomail)
                {
                    _notification.AddRecommendNotification(userid, users.UserId, storyid, 3);
                    try
                    {
                       
                        string email = users.Email;
                        var link = "<a href=\"https://localhost:5001/Story/StoryDetails?storyid=" + storyid + "\">Story Link</a>";
                        MailMessage newMail = new MailMessage();
                        SmtpClient client = new SmtpClient("smtp.gmail.com");
                        newMail.From = new MailAddress("ciplatform333@gmail.com", "CI Platform");
                        newMail.To.Add(email);
                        newMail.Subject ="Story Recommended by " + currentuser.FirstName + " " + currentuser.LastName ;
                        newMail.IsBodyHtml = true;
                        newMail.Body =currentuser.FirstName+" " +currentuser.LastName + "Recommended you the below story<br><br><br>" + link;
                        client.EnableSsl = true;
                        client.Port = 587;
                        client.Credentials = new System.Net.NetworkCredential("ciplatform333@gmail.com", "jbdxshjsnfhyimnp");
                        client.Send(newMail);



                       

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return true;
            }
            return false;
        }







        [HttpPost]
        public IActionResult Submit(ShareStoryVm item)
        {
            string? user = HttpContext.Session.GetString("UserId");
            long? userid = long.Parse(user);
            var submitcondition = _db.Stories.Where(x => x.MissionId == item.MissionId && x.UserId == (long)userid && x.Status=="DRAFT").FirstOrDefault();
            if (submitcondition != null)
            {

                //Story story = new Story();
                //story.MissionId = missionId;
                //story.Status = "PENDING";
                //story.Title = StoryTitle;
                //story.Description = StoryDescription;
                //story.CreatedAt = Date;
                //_db.Stories.Add(story);
                submitcondition.Status = "PENDING";
                submitcondition.Title = item.StoryTitle;
                submitcondition.Description = item.StoryDesctiption;
                submitcondition.CreatedAt = item.Date;
                var storymedia = _db.StoryMedia.Where(x => x.StoryId == submitcondition.StoryId).ToList();
                if(storymedia != null)
                {
                    _db.StoryMedia.RemoveRange(storymedia);
                    _db.SaveChanges();
                }
                //image input
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (item.Storyimg != null)
                {
                    foreach (var img in item.Storyimg)
                    {
                        StoryMedium storyMedia = new StoryMedium();
                        storyMedia.StoryId = submitcondition.StoryId;
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"assets\storyImages");
                        var extension = Path.GetExtension(img.FileName);

                        using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            img.CopyTo(filestream);
                        }
                        storyMedia.Type = extension;
                        storyMedia.Path =  fileName + extension;

                        _db.Add(storyMedia);
                        _db.SaveChanges(true);
                    }
                }
                //image input


                //video url input
                if (item.VideoUrl != null)
                {
                    string[] videopath = item.VideoUrl.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    if (videopath.Length <= 20)
                    {
                        foreach (var v in videopath)
                        {
                            StoryMedium video = new StoryMedium();
                            video.StoryId = submitcondition.StoryId;
                            video.Type = "video";
                            video.Path = v;
                            _db.StoryMedia.Add(video);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.MaxVideo = "Cannot upload more than 20 Url";
                    }

                }
                // video url input
                _db.Stories.Update(submitcondition);
                _db.SaveChanges();
                return RedirectToAction("ShareYourStory");
            }
            return View(item);
           
        }








        //public JsonResult DraftedData(long missionid)
        //{
        //    string? user = HttpContext.Session.GetString("UserId");
        //    long? userid = long.Parse(user);
        //    //ShareStoryVm draftedData = new ShareStoryVm();
        //    ShareStoryVm draftdata = new ShareStoryVm();
        //    var saveddata = _db.Stories.FirstOrDefault(x=> x.MissionId == missionid && x.UserId== userid && x.Status=="DRAFT");
        //    if(saveddata != null)
        //    {
        //        draftdata.StoryTitle = saveddata.Title;
        //        draftdata.StoryDesctiption = saveddata.Description;
        //        draftdata.Date = saveddata.CreatedAt;

        //        var savedmedia = _db.StoryMedia.Where(x => x.StoryId == saveddata.StoryId).ToList();
        //        List<VideoListVm> videopaths = new List<VideoListVm>();
        //        foreach (var media in savedmedia)
        //        {
        //            var video = new VideoListVm();
        //            if (media.Type == "video")
        //            {
        //                video.VideoPath = media.Path;
        //                videopaths.Add(video);
        //            }
        //            //else
        //            //{
        //            //    draftedData.ImagePath = media.Path;
        //            //}
        //        }
        //        draftdata.VideoList = videopaths;

        //        return new JsonResult(draftdata);
        //    }
        //    else { return new JsonResult(null); }


        //}

        public IActionResult DraftedData(long missionid)
        {
            string? user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            //ShareStoryVm draftedData = new ShareStoryVm();
            ShareStoryVm draftdata = new ShareStoryVm();
            ShareStoryVm? filldropdown = _storyCards.GetUserMissions(userid);
            draftdata.UserAppliedMissions = filldropdown.UserAppliedMissions;
            Story? saveddata = _db.Stories.FirstOrDefault(x => x.MissionId == missionid && x.UserId == userid && x.Status == "DRAFT");
            if (saveddata != null)
            {
                draftdata.StoryId = saveddata.StoryId;
                draftdata.StoryTitle = saveddata.Title;
                draftdata.StoryDesctiption = saveddata.Description;
                draftdata.Date = saveddata.CreatedAt;

                var savedmedia = _db.StoryMedia.Where(x => x.StoryId == saveddata.StoryId).ToList();
                List<string> yturls = new List<string>();
                List<string> savedimgs = new List<string>();
                foreach (var media in savedmedia)
                {
                    if (media.Type == "video")
                    {
                        yturls.Add(media.Path);
                    }

                    if(media.Type == ".png" || media.Type == ".jpg" || media.Type == ".jpeg")
                    {
                        savedimgs.Add(media.Path);
                    }
                }
                string[] youtubeurls = yturls.ToArray();

                draftdata.VideoUrl= string.Join(Environment.NewLine,youtubeurls);
                if(savedimgs.Count > 0)
                {
                    draftdata.ImagePath = savedimgs;
                }

                


                return PartialView("_ShareYourStoryPartial", draftdata);
            }
            else { return PartialView("_ShareYourStoryPartial", draftdata); }


        }







        [HttpPost]
        public IActionResult ShareYourStory(ShareStoryVm obj)
        {
            string? user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            if (obj.MissionId != 0 && obj.MissionId != null)
            {
               
                var savecondition = _db.Stories.Where(x => x.MissionId == obj.MissionId && x.UserId == userid && x.Status=="DRAFT").FirstOrDefault();
                if (savecondition == null)
                {
                    
                    Story story = new Story();

                    story.MissionId = obj.MissionId;
                    story.UserId = userid;
                    story.Status = "DRAFT";
                    story.Title = obj.StoryTitle;
                    story.Description = obj.StoryDesctiption;
                    story.CreatedAt = obj.Date;

                    _db.Stories.Add(story);
                    _db.SaveChanges(true);

                    //image input
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    if (obj.Storyimg != null)
                    {
                        foreach (var img in obj.Storyimg)
                        {
                            StoryMedium storyMedia = new StoryMedium();
                            storyMedia.StoryId = story.StoryId;
                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"assets\storyImages");
                            var extension = Path.GetExtension(img.FileName);

                            using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                            {
                                img.CopyTo(filestream);
                            }
                            storyMedia.Type = extension;
                            storyMedia.Path = @"\assets\storyImages\" + fileName + extension;
                            
                            _db.Add(storyMedia);
                            _db.SaveChanges(true);
                        }
                    }
                    //image input

                    //video url input
                    if (obj.VideoUrl != null)
                    {
                        string[] videopath = obj.VideoUrl.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        if (videopath.Length <= 20)
                        {
                            foreach (var v in videopath)
                            {
                                StoryMedium video = new StoryMedium();
                                video.StoryId = story.StoryId;
                                video.Type = "video";
                                video.Path = v;
                                _db.StoryMedia.Add(video);
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            ViewBag.MaxVideo = "Cannot upload more than 20 Url";
                        }

                    }
                    // video url input
                    obj.UserAppliedMissions = _storyCards.GetUserMissions(userid).UserAppliedMissions;
                    
                    return View(obj);
                }




                else if (savecondition != null)
                {
                    ViewBag.DraftedStory = savecondition;

                    
                    savecondition.Title = obj.StoryTitle;
                    savecondition.Description = obj.StoryDesctiption;
                    savecondition.CreatedAt = obj.Date;
                    var savedmedia = _db.StoryMedia.Where(x=> x.StoryId == savecondition.StoryId).ToList();
                    if (savedmedia != null)
                    {
                        _db.StoryMedia.RemoveRange(savedmedia);
                        _db.SaveChanges();
                    }

                    //image input
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    if (obj.Storyimg != null)
                    {
                        foreach (var img in obj.Storyimg)
                        {
                            StoryMedium storyMedia = new StoryMedium();
                            storyMedia.StoryId = savecondition.StoryId;
                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"assets\storyImages");
                            var extension = Path.GetExtension(img.FileName);

                            using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                            {
                                img.CopyTo(filestream);
                            }
                            storyMedia.Type = extension;
                            storyMedia.Path = @"\assets\storyImages\" + fileName + extension;

                            _db.Add(storyMedia);
                            _db.SaveChanges(true);
                        }
                    }
                    //image input

                    //video url input
                    if (obj.VideoUrl != null)
                    {
                        string[] videopath = obj.VideoUrl.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        if (videopath.Length <= 20)
                        {
                            foreach (var v in videopath)
                            {
                                StoryMedium video = new StoryMedium();
                                video.StoryId = savecondition.StoryId;
                                video.Type = "video";
                                video.Path = v;
                                _db.StoryMedia.Add(video);
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            ViewBag.MaxVideo = "Cannot upload more than 20 Url";
                        }

                    }
                    // video url input


                    _db.Stories.Update(savecondition);
                    _db.SaveChanges(true);
                    obj.UserAppliedMissions = _storyCards.GetUserMissions(userid).UserAppliedMissions;
                    //obj.StoryTitle = null;
                    return View(obj);
                }
               
            }
            ViewBag.SelectMission = "Please Select a Mission";
            obj.UserAppliedMissions = _storyCards.GetUserMissions(userid).UserAppliedMissions;
            return View(obj);
            
            
        }



        public bool ImageDelete(long id, string source)
        {
            StoryMedium? image = _db.StoryMedia.FirstOrDefault(x => x.StoryId == id && x.Path == source && x.DeletedAt == null);
            if (image != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var filepath = Path.GetFullPath(Path.Combine(wwwRootPath, @"assets\MissionMedia\Images\" + image.Path));
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                image.DeletedAt = DateTime.Now;
                _db.StoryMedia.Update(image);
                _db.SaveChanges();
                return true;
            }
            return false;
        }





    }
}
