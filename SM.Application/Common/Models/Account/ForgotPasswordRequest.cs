using System.ComponentModel.DataAnnotations;

namespace SM.Application.Common.Models.Account
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
