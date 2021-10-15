using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Runtime;

public sealed class IdentifyingText : Result<string>
{
	public static IdentifyingText Default { get; } = new IdentifyingText();

	IdentifyingText() : base(() => Guid.NewGuid().ToString()) {}

	public override string ToString() => Get();
}