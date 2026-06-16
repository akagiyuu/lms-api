using FluentValidation;
using PRN232.LMS.Services.Models.Request;

namespace PRN232.LMS.Services.Validation;

// FluentValidation validator for CreateStudentRequest
public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            // Custom rule: must be a valid FPT University email
            .Must(e => e != null && e.EndsWith("@fpt.edu.vn"))
            .WithMessage("Email must be an FPT University address (e.g. abc@fpt.edu.vn).");

        RuleFor(x => x.DateOfBirth)
            .NotNull().WithMessage("Date of birth is required.")
            .Must(d => d < DateTimeOffset.UtcNow).WithMessage("Date of birth must be in the past.");
    }
}
