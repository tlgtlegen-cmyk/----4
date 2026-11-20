namespace AishaTulegen.ViewModelValidation.ViewModels;

public sealed class UserDetailsViewModel
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public int Age { get; init; }
    public DateTime CreatedAt { get; init; }
}
