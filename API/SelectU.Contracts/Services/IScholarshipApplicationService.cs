﻿using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipApplicationService
    {
        Task<ScholarshipApplicationUpdateDTO> GetScholarshipApplicationAsync(Guid id);
        Task<List<ScholarshipApplicationUpdateDTO>> GetMyScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, string id, bool isAdmin);
        Task<ResponseDTO> CreateScholarshipApplicationAsync(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO, string id);
        Task<ResponseDTO> CreateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO);
        Task<ResponseDTO> UpdateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO);
        Task<ResponseDTO> DeleteScholarshipApplicationRatingAsync(Guid ScholarshipId, Guid RatingId);

    }
}
