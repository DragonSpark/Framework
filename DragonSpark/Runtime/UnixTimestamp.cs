using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime;

public sealed class UnixTimestamp : ISelect<DateTime, ulong>
{
	public static UnixTimestamp Default { get; } = new UnixTimestamp();

	UnixTimestamp() : this(DateTime.UnixEpoch) {}

	readonly DateTime _epoch;

	public UnixTimestamp(DateTime epoch) => _epoch = epoch;

	public ulong Get(DateTime parameter)
	{
		var diff   = parameter - _epoch;
		var result = (ulong)Math.Floor(diff.TotalSeconds);
		return result;
	}
}