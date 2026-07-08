using DragonSpark.Contracts.Worker;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.AspNet.Worker.Processes;

[Owned, Index(nameof(Status))]
public sealed record ProcessState(
	DateTimeOffset Last,
	[property: MaxLength(512)]
	string? Message = "This process is being queued for processing",
	ProcessStatus Status = ProcessStatus.Queued)
{
	public ProcessState() : this(Time.Default) {}
}