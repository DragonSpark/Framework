namespace DragonSpark.Compose.Extents.Conditions
{
	public sealed class DefaultConditionExtent<T> : ConditionExtent<T>
	{
		public static DefaultConditionExtent<T> Default { get; } = new DefaultConditionExtent<T>();

		DefaultConditionExtent() {}
	}
}