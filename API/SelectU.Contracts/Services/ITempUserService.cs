using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface ITempUserService
    {
        Task<ValidateUniqueEmailAddressResponseDTO> ValidateUniqueEmailAddressAsync(string emailAddress);

        Task InviteTempUserAsync();

        Task UpdateTempUserExpiryAsync();


    }
}
