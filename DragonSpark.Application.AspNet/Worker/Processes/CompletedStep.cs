using System;

namespace DragonSpark.Application.AspNet.Worker.Processes;

public sealed class CompletedStep
{
	public uint Id { get; set; }

	public Guid Identifier { get; init; }
}