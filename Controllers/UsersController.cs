using AishaTulegen.ViewModelValidation.Data;
using AishaTulegen.ViewModelValidation.Models;
using AishaTulegen.ViewModelValidation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AishaTulegen.ViewModelValidation.Controllers;

public sealed class UsersController : Controller
{
    private readonly ApplicationDbContext _database;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersController(ApplicationDbContext database, IPasswordHasher<User> passwordHasher)
    {
        _database = database;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var normalizedEmail = model.Email.Trim().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(model.Email)
            && await _database.Users.AnyAsync(user => user.Email.ToLower() == normalizedEmail))
        {
            ModelState.AddModelError(nameof(model.Email), "Бұл электрондық пошта бұрын тіркелген.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            FullName = model.FullName.Trim(),
            Email = normalizedEmail,
            Age = model.Age!.Value,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = string.Empty
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        _database.Users.Add(user);
        try
        {
            await _database.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (
            exception.InnerException is SqliteException { SqliteErrorCode: 19 } sqliteException
            && sqliteException.Message.Contains("Users.Email", StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(model.Email), "Бұл электрондық пошта бұрын тіркелген.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Тіркелу сәтті аяқталды. Қош келдіңіз!";
        return RedirectToAction(nameof(List));
    }

    [HttpGet]
    public async Task<IActionResult> List(string? search)
    {
        var query = _database.Users.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLowerInvariant();
            query = query.Where(user =>
                user.FullName.ToLower().Contains(term) || user.Email.ToLower().Contains(term));
        }

        ViewData["Search"] = search?.Trim() ?? string.Empty;
        ViewData["TotalUsers"] = await _database.Users.CountAsync();
        var users = await query
            .OrderByDescending(user => user.CreatedAt)
            .Select(user => new UserListItemViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Age = user.Age,
                CreatedAt = user.CreatedAt
            })
            .ToListAsync();

        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var user = await _database.Users.AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new UserDetailsViewModel
            {
                FullName = item.FullName,
                Email = item.Email,
                Age = item.Age,
                CreatedAt = item.CreatedAt
            })
            .SingleOrDefaultAsync();

        return user is null ? NotFound() : View(user);
    }
}
