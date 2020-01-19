using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	sealed class EndpointConfiguration : ICommand<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() : this(DefaultApplicationSelector.Default, "/_Host") {}

		readonly string _selector, _fallback;

		public EndpointConfiguration(string selector, string fallback)
		{
			_selector = selector;
			_fallback = fallback;
		}

		public void Execute(IEndpointRouteBuilder parameter)
		{
			parameter.MapBlazorHub(_selector)
			         .Return(parameter)
			         .MapFallbackToPage(_fallback);
		}
	}
}