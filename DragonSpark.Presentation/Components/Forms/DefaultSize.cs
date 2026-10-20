namespace DragonSpark.Presentation.Components.Forms;

public sealed class DefaultSize : DragonSpark.Model.Results.Instance<byte>
{
	public static DefaultSize Default { get; } = new();

	DefaultSize() : base(60) {}
}