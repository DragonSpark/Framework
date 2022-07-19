using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Runtime;

public sealed class CurrentUnixTimestamp : SelectedResult<DateTime, ulong>
{
	public static CurrentUnixTimestamp Default { get; } = new();

	CurrentUnixTimestamp() : this(Time.Default) {}

	public CurrentUnixTimestamp(ITime time)
		: base(time.Then().Select(x => x.UtcDateTime).Get(), UnixTimestamp.Default) {}
}