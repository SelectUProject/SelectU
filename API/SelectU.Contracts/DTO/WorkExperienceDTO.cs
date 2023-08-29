using SelectU.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.DTO
{
    public class WorkExperienceDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? OnGoing { get; set; }
        public WorkExperience ToWorkExperience()
        {
            return new WorkExperience { Name = Name, Description = Description, StartDate = StartDate, EndDate = EndDate, OnGoing = OnGoing};
        }
    }
}
