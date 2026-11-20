using AishaTulegen.ViewModelValidation.Data;
using AishaTulegen.ViewModelValidation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AishaTulegen.ViewModelValidation.Controllers;

public sealed class HomeController : Controller
{
    private readonly ApplicationDbContext _database;

    public HomeController(ApplicationDbContext database)
    {
        _database = database;
    }

    public async Task<IActionResult> Index()
    {
        var total = await _database.Users.CountAsync();
        return View(new DashboardViewModel
        {
            TotalUsers = total,
            SuccessfulRegistrations = total,
            RecentUsers = await _database.Users
                .OrderByDescending(user => user.CreatedAt)
                .Take(4)
                .Select(user => new UserListItemViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Age = user.Age,
                    CreatedAt = user.CreatedAt
                })
                .ToListAsync()
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
