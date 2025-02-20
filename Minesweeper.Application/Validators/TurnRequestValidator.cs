using FluentValidation;
using Minesweeper.Application.DTOs;

namespace Minesweeper.Application.Validators;

public class TurnRequestValidator : AbstractValidator<TurnRequestDto>
{
    public TurnRequestValidator()
    {
        RuleFor(x => x.GameId).NotEmpty();
        RuleFor(x => x.Row).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Col).GreaterThanOrEqualTo(0);
    }
}