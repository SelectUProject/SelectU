using SelectU.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Services
{
    public interface IReviewService
    {
        Task CreateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO);
        Task UpdateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO, bool isAdmin);
        Task DeleteScholarshipApplicationRatingAsync(Guid RatingId, string CreatorId, bool isAdmin);
    }
}
