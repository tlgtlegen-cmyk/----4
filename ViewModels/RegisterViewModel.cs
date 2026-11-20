using System.ComponentModel.DataAnnotations;
using AishaTulegen.ViewModelValidation.Validation;

namespace AishaTulegen.ViewModelValidation.ViewModels;

// Тіркеу ViewModel-і тек формаға қажет деректерді ұстайды; домендегі PasswordHash ешқашан көрініске берілмейді.
public sealed class RegisterViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Аты-жөніңізді енгізіңіз.")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Аты-жөні 5 пен 100 таңба аралығында болуы керек.")]
    [RegularExpression(@"^[A-Za-zА-Яа-яЁёӘәҒғҚқҢңӨөҰұҮүҺһІі][A-Za-zА-Яа-яЁёӘәҒғҚқҢңӨөҰұҮүҺһІі\s'-]*$", ErrorMessage = "Аты-жөнінде тек әріптер мен бос орын қолданылуы керек.")]
    [Display(Name = "Аты-жөні")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Электрондық поштаңызды енгізіңіз.")]
    [EmailAddress(ErrorMessage = "Электрондық пошта пішімі дұрыс емес.")]
    [StringLength(254, ErrorMessage = "Электрондық пошта 254 таңбадан аспауы керек.")]
    [Display(Name = "Электрондық пошта")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Жасыңызды енгізіңіз.")]
    [Range(16, 100, ErrorMessage = "Жас 16 мен 100 аралығында болуы керек.")]
    [Display(Name = "Жасы")]
    public int? Age { get; set; }

    [Required(ErrorMessage = "Құпиясөзді енгізіңіз.")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Құпиясөз кемінде 8 таңбадан тұруы керек.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "Құпиясөзде кемінде бір латын әрпі мен бір сан болуы керек.")]
    [StrongPassword]
    [DataType(DataType.Password)]
    [Display(Name = "Құпиясөз")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпиясөзді растаңыз.")]
    [Compare(nameof(Password), ErrorMessage = "Құпиясөздер бірдей емес.")]
    [DataType(DataType.Password)]
    [Display(Name = "Құпиясөзді растау")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var parts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length < 2)
        {
            yield return new ValidationResult("Аты-жөніңізді толық жазыңыз (аты және тегі).", [nameof(FullName)]);
        }

        if (Age is >= 16 and < 18)
        {
            yield return new ValidationResult("Тіркелу үшін 18 жасқа толған болуыңыз керек.", [nameof(Age)]);
        }
    }
}
