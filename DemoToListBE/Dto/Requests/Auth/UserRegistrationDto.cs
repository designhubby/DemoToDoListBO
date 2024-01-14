using System.ComponentModel.DataAnnotations;

namespace DemoToListBE.Dto.Requests.Auth
{
    public record UserRegistrationDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = String.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; } = String.Empty;
        [Required]
        [MaxLength(20)]
        public string Password { get; set; } = String.Empty;
        [MaxLength(20)]
        public string FirstName { get; set; } = String.Empty;
        [MaxLength(20)]
        public string LastName { get; set; } = String.Empty;



    }
}
