namespace DragonSpark.Sources.Parameterized.Caching
{
	public class Condition : Condition<object>
	{
		public new static Condition Default { get; } = new Condition();
	}

	public class Condition<T> : Cache<T, ConditionMonitor> where T : class
	{
		public static Condition<T> Default { get; } = new Condition<T>();
		public Condition() : base( key => new ConditionMonitor() ) {}
	}
}