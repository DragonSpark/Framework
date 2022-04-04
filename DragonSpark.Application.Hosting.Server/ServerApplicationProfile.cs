using DragonSpark.Application.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server;

public sealed class ServerApplicationProfile : ApplicationProfile
{
	public static ServerApplicationProfile Default { get; } = new();

	ServerApplicationProfile() : this(DefaultApplicationConfiguration.Default) {}

	public ServerApplicationProfile(ICommand<IApplicationBuilder> application)
		: base(DefaultServiceConfiguration.Default, application) {}
}