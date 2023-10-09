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
        Task CreateReviewAsync(ReviewDTO reviewDTO);
        Task UpdateReviewAsync(ReviewDTO reviewDTO, bool isAdmin);
        Task DeleteReviewAsync(Guid reviewId, string creatorId, bool isAdmin);
    }
}
