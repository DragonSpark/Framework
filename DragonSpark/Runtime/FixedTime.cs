using DragonSpark.Model.Results;

namespace DragonSpark.Runtime;

public class FixedTime : Instance<DateTimeOffset>, ITime
{
	public FixedTime(DateTimeOffset instance) : base(instance) {}
}