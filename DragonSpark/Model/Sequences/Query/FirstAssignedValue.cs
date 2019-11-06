namespace DragonSpark.Model.Sequences.Query
{
	public sealed class FirstAssignedValue<T> : FirstWhere<T?> where T : struct
	{
		public static FirstAssignedValue<T> Default { get; } = new FirstAssignedValue<T>();

		FirstAssignedValue() : base(x => x != null) {}
	}
}