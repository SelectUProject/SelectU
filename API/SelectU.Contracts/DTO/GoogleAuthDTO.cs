using System.ComponentModel.DataAnnotations;

namespace SelectU.Contracts.DTO
{
    public class GoogleAuthDTO
    {
        [Required(ErrorMessage = "Id Token is required")]
        public string? IdToken { get; set; }

    }
}
