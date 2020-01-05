using DragonSpark.Application.Hosting.Server.GitHub.Testing.Application.Controllers;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.GitHub.Testing.Application
{
	public sealed class IntegrationTests
	{
		
	}

	sealed class Registrations : Command<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() : base(x => x.AddSingleton(Start.A.Registration()
		                                                .For.IssueComments()
		                                                .With<CustomHandler>()
		                                                .Get)
		                             .AddScoped<CustomHandler>()) {}
	}
}