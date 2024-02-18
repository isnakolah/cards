using System.Text.RegularExpressions;
using AutoMapper;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cards.Application.Cards.Commands;

public sealed record CreateCardCommand(
        string Name,
        string? Description,
        string? Color) 
    : IRequest<ApiResponse<CardVm>>;

public sealed class CreateCardCommandHandler(
        ILogger<CreateCardCommandHandler> _logger,
        ICurrentUserService currentUserService,
        IApplicationDbContext dbContext,
        IMapper mapper)
    : IRequestHandler<CreateCardCommand, ApiResponse<CardVm>>
{
    public async Task<ApiResponse<CardVm>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([currentUserService.UserId], cancellationToken);
        
        if (user is null)
        {
            return ApiResponse<CardVm>.Error("User not found");
        }

        try
        {
            user.AddCard(
                name: request.Name,
                description: request.Description,
                color: request.Color);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating card");

            return ApiResponse<CardVm>.Error(e.Message);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return ApiResponse<CardVm>.Success(mapper.Map<CardVm>(user.Cards.Last()));
    }
}

public sealed partial class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
{
    public CreateCardCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(command => command.Description)
            .MaximumLength(500);

        RuleFor(command => command.Color)
            .Must(BeAValidColor);
    }
    
    private bool BeAValidColor(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return true;
        }

        var regex = MyRegex();

        return regex.IsMatch(color.Trim());
    }

    [GeneratedRegex("^#[A-Za-z0-9]{6}$")]
    private static partial Regex MyRegex();
}
