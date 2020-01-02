namespace DragonSpark.Model.Selection.Conditions
{
	sealed class Always<T> : FixedResultCondition<T>
	{
		public static ICondition<T> Default { get; } = new Always<T>();

		Always() : base(true) {}
	}
}