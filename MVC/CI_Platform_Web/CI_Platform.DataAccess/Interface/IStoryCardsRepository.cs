using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IStoryCardsRepository
    {
        public List<StoryListingVm> GetStories();

        public ShareStoryVm GetUserMissions(long userid);

        public StoryDetailsVm GetStoryDetails(long stroyid);
    }
}
