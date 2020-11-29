using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Hosting.Console
{
	public static class Extensions
	{
		public static ApplicationProfileContext WithConsoleApplication(this BuildHostContext @this)
			=> @this.Apply(ConsoleApplicationProfile.Default);
	}
}