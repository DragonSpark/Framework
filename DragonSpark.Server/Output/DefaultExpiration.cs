using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Server.Output;

public sealed class DefaultExpiration : Instance<TimeSpan>
{
	public static DefaultExpiration Default { get; } = new();

	DefaultExpiration() : base(TimeSpan.FromHours(1)) {}
}