using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminStoryRepository
    {
        public List<AdminStoryDisplayVm> GetStories();

        public AdminStoryDetailsVm GetDetails(long storyid);

        public string StoryApprove(long storyid);

        public string StoryDecline(long storyid);

        public string StoryDelete(long storyid);
    }
}
