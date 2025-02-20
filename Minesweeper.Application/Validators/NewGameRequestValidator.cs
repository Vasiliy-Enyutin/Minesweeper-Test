using FluentValidation;
using Minesweeper.Application.DTOs;

namespace Minesweeper.Application.Validators;

public class NewGameRequestValidator : AbstractValidator<NewGameRequestDto>
{
    public NewGameRequestValidator()
    {
        RuleFor(x => x.Width)
            .InclusiveBetween(2, 30)
            .WithMessage("Width must be between 2 and 30");

        RuleFor(x => x.Height)
            .InclusiveBetween(2, 30)
            .WithMessage("Height must be between 2 and 30");

        RuleFor(x => x.MinesCount)
            .Must((request, mines) => mines > 0 && mines <= request.Width * request.Height - 1)
            .WithMessage("Invalid mines count");
    }
}