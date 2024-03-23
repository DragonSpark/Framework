using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Application.Hosting.Console;

public static class Extensions
{
	[UsedImplicitly]
	public static ApplicationProfileContext WithConsoleApplication(this BuildHostContext @this)
		=> @this.Apply(ConsoleApplicationProfile.Default);
}