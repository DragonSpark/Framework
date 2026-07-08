using System;

namespace DragonSpark.Application.AspNet.Workers;

public sealed class CompletedStep
{
	public uint Id { get; set; }

	public Guid Identifier { get; init; }
}