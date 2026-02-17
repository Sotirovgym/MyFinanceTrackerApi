using System.ComponentModel.DataAnnotations;

namespace MyFinanceTracker.Application.Features.Authentication.DTOs
{
    /// <summary>
    /// Represents the data required for a user to log in.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The password of the user.
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
    }
}
