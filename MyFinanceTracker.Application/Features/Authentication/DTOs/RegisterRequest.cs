using System.ComponentModel.DataAnnotations;

namespace MyFinanceTracker.Application.Features.Authentication.DTOs
{
    /// <summary>
    /// Represents the data required to register a new user.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        public string Email { get; init; } = null!;

        /// <summary>
        /// The password chosen by the user.
        /// </summary>
        [Required]
        public string Password { get; init; } = null!;

        /// <summary>
        /// The confirmation of the password to ensure it matches.
        /// </summary>
        [Required]
        public string ConfirmPassword { get; init; } = null!;

        /// <summary>
        /// Indicates whether the user wants to enable notifications.
        /// </summary>
        public bool EnableNotifications { get; set; }
    }
}
