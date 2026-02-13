namespace MyFinanceTracker.Application.Features.Authentication.DTOs
{
    public class RegisterRequest
    {
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string ConfirmPassword { get; init; } = null!;
        public bool EnableNotifications { get; set; }
    }
}
