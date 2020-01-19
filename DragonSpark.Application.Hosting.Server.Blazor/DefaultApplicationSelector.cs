using DragonSpark.Model.Results;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	sealed class DefaultApplicationSelector : Instance<string>
	{
		public static DefaultApplicationSelector Default { get; } = new DefaultApplicationSelector();

		DefaultApplicationSelector() : base("/_blazor") {}
	}
}