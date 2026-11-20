namespace AishaTulegen.ViewModelValidation.ViewModels;

public sealed class DashboardViewModel
{
    public int TotalUsers { get; init; }
    public int SuccessfulRegistrations { get; init; }
    public required IReadOnlyList<UserListItemViewModel> RecentUsers { get; init; }
}
