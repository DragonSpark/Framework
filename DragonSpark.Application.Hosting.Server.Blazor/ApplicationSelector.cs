using DragonSpark.Model.Results;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class ApplicationSelector : Instance<string>
{
	public static ApplicationSelector Default { get; } = new();

	ApplicationSelector() : base("div#application") {}
}