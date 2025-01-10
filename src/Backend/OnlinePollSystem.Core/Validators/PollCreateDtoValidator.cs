using FluentValidation;
using OnlinePollSystem.Core.DTOs.Poll;

namespace OnlinePollSystem.Core.Validators
{
    public class PollCreateDtoValidator : AbstractValidator<PollCreateDto>
    {
        public PollCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Poll title is required")
                .MinimumLength(3).WithMessage("Poll title must be at least 3 characters")
                .MaximumLength(200).WithMessage("Poll title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must(BeAValidDate).WithMessage("Invalid start date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

            RuleFor(x => x.Options)
                .NotEmpty().WithMessage("Poll must have at least two options")
                .Must(options => options.Count >= 2).WithMessage("Poll must have at least two options")
                .Must(options => options.Distinct().Count() == options.Count).WithMessage("Options must be unique");

            RuleForEach(x => x.Options)
                .NotEmpty().WithMessage("Option cannot be empty")
                .MaximumLength(200).WithMessage("Option cannot exceed 200 characters");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date > DateTime.UtcNow.AddDays(-1) && date < DateTime.UtcNow.AddYears(2);
        }
    }
}