namespace DragonSpark.Presentation.Components.Content.Sequences;

public sealed class DefaultPageSize : DragonSpark.Model.Results.Instance<byte>
{
	public static DefaultPageSize Default { get; } = new();

	DefaultPageSize() : base(4) {}
}