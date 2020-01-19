using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime
{
	public sealed class UnixTicks : ISelect<DateTime, double>
	{
		public static UnixTicks Default { get; } = new UnixTicks();

		UnixTicks() : this(UnixEpoch.Default.Get()) {}

		readonly DateTime _epoch;

		public UnixTicks(DateTime epoch) => _epoch = epoch;

		public double Get(DateTime parameter)
		{
			var diff   = parameter - _epoch;
			var result = Math.Floor(diff.TotalSeconds);
			return result;
		}
	}
}