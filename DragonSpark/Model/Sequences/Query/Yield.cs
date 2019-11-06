namespace DragonSpark.Model.Sequences.Query
{
	public sealed class Yield<T> : Model.Selection.Select<T, T[]>
	{
		public static Yield<T> Default { get; } = new Yield<T>();

		Yield() : base(x => new[] {x}) {}
	}
}