namespace DragonSpark.Application.Security.Identity
{
	public sealed class Anonymous : Text.Text
	{
		public static Anonymous Default { get; } = new Anonymous();

		Anonymous() : base(nameof(Anonymous)) {}
	}
}
