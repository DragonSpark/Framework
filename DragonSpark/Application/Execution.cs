using DragonSpark.Sources;

namespace DragonSpark.Application
{
	public sealed class Execution : SuppliedSource<ISource>
	{
		public static Execution Default { get; } = new Execution();
		Execution() : base( ExecutionContext.Default ) {}
	}
}