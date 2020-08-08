using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Testing.Objects
{
	public class FixedTime : Instance<DateTimeOffset>, ITime
	{
		public FixedTime(DateTimeOffset instance) : base(instance) {}
	}
}