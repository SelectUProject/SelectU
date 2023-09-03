using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipApplicationService
    {
        Task<ScholarshipApplication> GetScholarshipApplicationAsync(Guid id);
        Task<List<ScholarshipApplication>> GetActiveScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO);
    }
}
