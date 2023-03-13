﻿global using AutoMapper;
global using FluentValidation;
global using FluentValidation.Results;
global using MediatR;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using StarWars.JediArchives.Application.Contracts.Infrastructure;
global using StarWars.JediArchives.Application.Contracts.Persistence;
global using StarWars.JediArchives.Application.Exceptions;
global using StarWars.JediArchives.Application.Features.Common.Dto;
global using StarWars.JediArchives.Application.Features.Common.Pagination;
global using StarWars.JediArchives.Application.Features.Timelines.Commands.CreateTimeline;
global using StarWars.JediArchives.Application.Features.Timelines.Commands.UpdateTimeline;
global using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail;
global using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList;
global using StarWars.JediArchives.Domain.Models;
global using StarWars.JediArchives.Infrastructure.QueryParser;
global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq;
global using System.Reflection;
global using System.Threading;
global using System.Threading.Tasks;
global using ValidationException = StarWars.JediArchives.Application.Exceptions.ValidationException;