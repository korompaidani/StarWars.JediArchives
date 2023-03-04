﻿using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using StarWars.DataTank.Application.Contracts.Persistence;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.CreateTimeline
{
    public class CreateTimelineCommandValidator : AbstractValidator<CreateTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;

        public CreateTimelineCommandValidator(ITimelineRepository timelineRepository)
        {
            _timelineRepository = timelineRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(e => e)
                .MustAsync(TimelineNameIsUnique)
                .WithMessage("An event name with the same name and date already exists.");

            RuleFor(p => p.StartYear)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .ExclusiveBetween(int.MinValue, int.MaxValue);

            RuleFor(p => p.EndYear)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .ExclusiveBetween(int.MinValue, int.MaxValue);
        }

        private async Task<bool> TimelineNameIsUnique(CreateTimelineCommand e, CancellationToken token)
        {
            return !(await _timelineRepository.IsTimelineNameUniqueAsync(e.Name));
        }
    }
}