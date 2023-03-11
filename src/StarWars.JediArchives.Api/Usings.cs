﻿global using MediatR;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.OpenApi.Models;
global using Newtonsoft.Json;
global using Serilog;
global using StarWars.JediArchives.Api.Middleware;
global using StarWars.JediArchives.Application;
global using StarWars.JediArchives.Application.Contracts.Infrastructure;
global using StarWars.JediArchives.Application.Exceptions;
global using StarWars.JediArchives.Application.Features.Timelines.Commands.CreateTimeline;
global using StarWars.JediArchives.Application.Features.Timelines.Commands.DeleteTimeline;
global using StarWars.JediArchives.Application.Features.Timelines.Commands.UpdateTimeline;
global using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail;
global using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList;
global using StarWars.JediArchives.Infrastructure.QueryParser;
global using StarWars.JediArchives.Persistence;
global using System;
global using System.Collections.Generic;
global using System.Net;
global using System.Threading.Tasks;
global using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;