namespace DragonSpark.Model.Sequences.Query
{
	public sealed class FirstAssigned<T> : FirstWhere<T> where T : class
	{
		public static FirstAssigned<T> Default { get; } = new FirstAssigned<T>();

		FirstAssigned() : base(x => x != null) {}
	}
}