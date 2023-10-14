using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipService
    {
        Task<List<ScholarshipUpdateDTO>> GetScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO);
        Task<Scholarship> GetScholarshipAsync(Guid id);
        Task<List<ScholarshipUpdateDTO>> GetActiveScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO);
        Task<List<ScholarshipUpdateDTO>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO, string userId);
        Task<ScholarshipUpdateDTO> CreateScholarshipAsync(ScholarshipCreateDTO scholarshipCreateDTO, string userId);
        Task UpdateScholarshipAsync(ScholarshipUpdateDTO scholarshipUpdateDTO);
        Task ArchiveScholarshipAsync(Guid id);
    }
}
