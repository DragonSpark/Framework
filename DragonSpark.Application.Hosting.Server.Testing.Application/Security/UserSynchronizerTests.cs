using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Testing.Server;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Hosting.Server.Testing.Application.Security
{
	public sealed class UserSynchronizerTests
	{
		sealed class Subject : UserSynchronizer<User>
		{
			[UsedImplicitly]
			public static Subject Default { get; } = new Subject();

			Subject() : base(x => x.Type.StartsWith(ClaimNamespace.Default)) {}
		}

		sealed class DisplayNameClaim : Claim
		{
			[UsedImplicitly]
			public static DisplayNameClaim Default { get; } = new DisplayNameClaim();

			DisplayNameClaim() : base("displayName") {}
		}

		class Claim : Text.Text
		{
			protected Claim(string name) : base($"{ClaimNamespace.Default}:{name}") {}
		}

		sealed class ClaimNamespace : Text.Text
		{
			public static ClaimNamespace Default { get; } = new ClaimNamespace();

			ClaimNamespace() : base("urn:testing") {}
		}

		sealed class ApplicationStorage : IdentityDbContext<User>
		{
			public ApplicationStorage(DbContextOptions<ApplicationStorage> options) : base(options) {}
		}

		sealed class User : IdentityUser
		{
			[UsedImplicitly]
			public string DisplayName { get; set; }
		}

		sealed class StorageBuilder : StorageBuilder<ApplicationStorage>
		{
			[UsedImplicitly]
			public static StorageBuilder Default { get; } = new StorageBuilder();

			StorageBuilder() : base(InMemoryConfiguration.Default.Execute) {}
		}

		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .Then(x => x.WithIdentity<User>()
			                                        .StoredIn<ApplicationStorage>()
			                                        .Using.Memory()) // TODO: Report
			                            .As.Is()
			                            .Operations()
			                            .Start();

			var users = host.Services.GetRequiredService<UserManager<User>>();
			users.Should().NotBeNull();

			/*var synchronized = await Subject.Default.Get((ClaimsPrincipal.Current, new Destination<User>()));
			synchronized.Should().BeTrue();*/
		}
	}
}