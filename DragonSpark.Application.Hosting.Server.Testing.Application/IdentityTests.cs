using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Testing.Server;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Hosting.Server.Testing.Application
{
	public sealed class IdentityTests
	{
		[Fact]
		public async ValueTask Verify()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithDefaultComposition()
			                            .RegisterModularity()
			                            .WithServerApplication()
			                            .As.Is()
			                            .Operations()
			                            .Start();

			await using var context = StorageBuilder.Default.Get();
		}

		sealed class ApplicationStorage : IdentityDbContext<User>
		{
			public ApplicationStorage(DbContextOptions<ApplicationStorage> options) : base(options) {}
		}

		sealed class User : IdentityUser
		{
			// ReSharper disable once UnusedMember.Local
			public string DisplayName { get; set; }
		}

		sealed class StorageBuilder : StorageBuilder<ApplicationStorage>
		{
			[UsedImplicitly]
			public static StorageBuilder Default { get; } = new StorageBuilder();

			StorageBuilder() : base(x => x.UseInMemoryDatabase(Guid.NewGuid().ToString())
			                               .EnableSensitiveDataLogging()) {}
		}
	}
}
