using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipService
    {
        Task<List<ScholarshipUpdateDTO>> GetScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO);
        Task<ScholarshipUpdateDTO> GetScholarshipAsync(Guid id);
        Task<List<ScholarshipUpdateDTO>> GetActiveScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO);
        Task<List<ScholarshipUpdateDTO>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO, string id);
        Task<ResponseDTO> CreateScholarshipAsync(ScholarshipCreateDTO scholarshipCreateDTO, string id);
        Task<ResponseDTO> UpdateScholarshipAsync(ScholarshipUpdateDTO scholarshipUpdateDTO);
        Task<ResponseDTO> DeleteScholarshipAsync(Guid id);
    }
}
