using DragonSpark.Model.Results;

namespace DragonSpark.Model.Sequences.Query
{
	public class LimitAware : Instance<Assigned<uint>>, ILimitAware
	{
		public LimitAware(Assigned<uint> instance) : base(instance) {}
	}
}