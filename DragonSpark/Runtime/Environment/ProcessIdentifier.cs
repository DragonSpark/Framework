using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Runtime.Environment;

public sealed class ProcessIdentifier : Result<Guid>
{
	public static ProcessIdentifier Default { get; } = new();

	ProcessIdentifier()
		: base(() => new(System.Environment.ProcessId.ToString("00000000-0000-0000-0000-000000000000"))) {}
}