namespace DragonSpark.Text
{
	public sealed class TextSelector<T> : Message<T>
	{
		public static TextSelector<T> Default { get; } = new TextSelector<T>();

		TextSelector() : base(x => x?.ToString() ?? string.Empty) {}
	}
}