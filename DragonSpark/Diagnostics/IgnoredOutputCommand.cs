namespace DragonSpark.Diagnostics
{
	public sealed class IgnoredOutputCommand : DelegatedTextCommand
	{
		public static IgnoredOutputCommand Default { get; } = new IgnoredOutputCommand();
		IgnoredOutputCommand() : base( s => {} ) {}
	}
}