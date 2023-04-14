using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionDisplayVm
    {
        public long MissionId { get; set; }

        //public long ThemeId { get; set; }

        //public long CityId { get; set; }

        //public long CountryId { get; set; }

        public string Title { get; set; } = null!;

        //public string? ShortDescription { get; set; }

        //public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        //public int? Status { get; set; }

        //public string? OrganizationName { get; set; }

        //public string? OrganizationDetails { get; set; }

        //public string? Availability { get; set; }

        //public DateTime CreatedAt { get; set; }

        //public DateTime? UpdatedAt { get; set; }

        //public DateTime? DeletedAt { get; set; }

        //public int? TotalSeats { get; set; }
    }
}
