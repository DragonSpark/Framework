namespace DragonSpark.Compose.Extents.Commands
{
	public sealed class DefaultCommandExtent<T> : CommandExtent<T>
	{
		public static DefaultCommandExtent<T> Default { get; } = new DefaultCommandExtent<T>();

		DefaultCommandExtent() {}
	}
}