using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime;

public sealed class UnixTime : ISelect<double, DateTime>
{
	public static UnixTime Default { get; } = new UnixTime();

	UnixTime() : this(DateTime.UnixEpoch) {}

	readonly DateTime _epoch;

	public UnixTime(DateTime epoch) => _epoch = epoch;

	public DateTime Get(double parameter) => _epoch.AddSeconds(parameter);
}