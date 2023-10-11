using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipService
    {
        Task<ScholarshipUpdateDTO> GetScholarshipAsync(Guid id);
        Task<List<ScholarshipUpdateDTO>> GetActiveScholarshipAsync(ScholarshipSearchDTO ScholarshipSearchDTO);
        Task<List<ScholarshipUpdateDTO>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO ScholarshipSearchDTO, string id);
        Task<ResponseDTO> CreateScholarshipAsync(ScholarshipCreateDTO scholarshipCreateDTO, string id);
        Task<ResponseDTO> UpdateScholarshipsAsync(ScholarshipUpdateDTO ScholarshipUpdateDTO);
        Task<ResponseDTO> DeleteScholarshipsAsync(Guid id);

        //Task UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO);
        //Task ChangePasswordAsync(string id, ChangePasswordDTO passwordDTO);
        //Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        //Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
