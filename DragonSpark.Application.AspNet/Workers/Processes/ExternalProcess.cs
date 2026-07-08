using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Workers.Processes;

[Index(nameof(Created), IsDescending = [true])]
public abstract class ExternalProcess
{
	public Guid Id { get; set; }

	public bool Enabled { get; set; } = true;

	public DateTimeOffset Created { get; set; }

	public DateTimeOffset? Completed { get; set; }

	public ICollection<CompletedStep> CompletedSteps { get; init; } = null!;

	public ICollection<ProcessUpdate> Updates { get; set; } = null!;

	public ProcessState State { get; set; } = null!;
}