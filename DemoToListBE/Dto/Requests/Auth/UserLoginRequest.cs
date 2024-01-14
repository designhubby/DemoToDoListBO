using System.ComponentModel.DataAnnotations;

namespace DemoToListBE.Dto.Requests.Auth
{
    public record UserLoginRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
