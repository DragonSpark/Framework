using System.Collections.Immutable;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Application.Hosting.Console
{
	sealed class ConsoleApplicationArgument : ApplicationArgument<ImmutableArray<string>>,
	                                          IActivateUsing<ImmutableArray<string>>
	{
		public ConsoleApplicationArgument(ImmutableArray<string> instance) : base(instance) {}
	}
}