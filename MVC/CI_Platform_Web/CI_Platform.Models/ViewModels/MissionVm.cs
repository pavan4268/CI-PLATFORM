﻿using CI_Platform.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionVm
    {
        public string? Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? OrganizationName { get; set; }

        public int? Rating { get; set; }

        public string? Img { get; set; }

        public string? MissionThemes { get; set; }

        public string? CityName { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; } 

        public int? NumberOfSeats { get; set; }

        public string? Deadline { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? Seats { get; set; }

        public int? AvailableSeats { get; set; }

        public string? MissionType { get; set; }

        public long? MissionId { get; set; }

        public bool? HasApplied { get; set; }

        public DateTime? RegistrationDeadline { get; set; }

        public long? CountryId { get; set; }

        public List<MissionSkill>? MissionSkill { get; set; }

        public int? Ratings { get; set; } = 0;

        public int GoalValue { get; set; } = 0;

        public string? GoalObjective { get; set; }

        public int GoalAchieved { get; set; } = 0;

        public float GoalPercentage { get; set; } = 0;

        public int AlreadyVolunteered { get; set; } = 0;

        public int MostFavourite { get; set; } = 0;

        public bool IsFavourite { get; set; } = false;
    }
}
