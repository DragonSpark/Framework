using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DragonSpark.Application.Hosting.Server;

public sealed class EndpointConfiguration : Command<IEndpointRouteBuilder>
{
	public static EndpointConfiguration Default { get; } = new();

	EndpointConfiguration() : base(x => x.MapControllers()) {}
}