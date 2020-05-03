namespace DragonSpark.Presentation.Components
{
	public sealed class Instance<T>
	{
		public T Value { get; set; } = default!;
	}
}