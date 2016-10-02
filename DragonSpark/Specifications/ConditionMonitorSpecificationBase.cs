using System;

namespace DragonSpark.Specifications
{
	public abstract class ConditionMonitorSpecificationBase<T> : SpecificationBase<T>
	{
		readonly Func<T, ConditionMonitor> source;
		protected ConditionMonitorSpecificationBase( Func<T, ConditionMonitor> source )
		{
			this.source = source;
		}

		public override bool IsSatisfiedBy( T parameter ) => source( parameter ).Apply();
	}
}