using DragonSpark.Contracts.Worker;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.AspNet.Workers.Processes;

[Index(nameof(Status))]
public sealed class ProcessUpdate
{
	public ulong Id { get; init; }

	public DateTimeOffset Created { get; init; }

	public ProcessStatus Status { get; init; }

	[MaxLength(512)]
	public string? Message { get; init; }
}