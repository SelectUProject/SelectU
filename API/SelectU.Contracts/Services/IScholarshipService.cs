using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IScholarshipService
    {
        Task<Scholarship> GetScholarshipAsync(Guid id);
        //Task<ScholarshipDTO[]> GetScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO);
        //Task CreateScholarshipAsync(ScholarshipCreateDTO createScholarshipDTO);
        //Task UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO);
        //Task ChangePasswordAsync(string id, ChangePasswordDTO passwordDTO);
        //Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        //Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
