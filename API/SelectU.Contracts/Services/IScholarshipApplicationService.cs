using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipApplicationService
    {
        Task<List<ScholarshipApplicationUpdateDTO>> GetScholarshipApplicationsAsync(Guid scholarshipId, ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO);
        Task<ScholarshipApplication> GetScholarshipApplicationAsync(Guid id);
        Task<List<ScholarshipApplicationUpdateDTO>> GetMyScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, string userId, bool isAdmin);
        Task CreateScholarshipApplicationAsync(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO, string userId);
        Task<ResponseDTO> SelectApplication(ScholarshipApplicationUpdateDTO scholarshipApplicationUpdateDTO, string id);
        Task<ScholarshipApplicationUpdateDTO?> GetNextReviewableApplication(Guid scholarshipId, string userId);

    }
}
