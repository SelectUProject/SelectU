using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipApplicationService
    {
        Task<ScholarshipApplication> GetScholarshipApplicationAsync(Guid id);
        Task<List<ScholarshipApplication>> GetMyScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, string id, bool isAdmin);
    }
}
