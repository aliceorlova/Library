using System.ComponentModel.DataAnnotations;

namespace WebApi.AuthModels
{
    public class AuthenticateModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
