using DragonSpark.Application.Hosting.Server;
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
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Security
{
	public sealed class UserSynchronizerTests
	{
		[Fact]
		public async Task VerifyAdd()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .Then(x => x.For<Subject>()
			                                        .Register.Scoped()
			                                        .WithIdentity<User>()
			                                        .StoredIn<ApplicationStorage>()
			                                        .Using.Memory())
			                            .As.Is()
			                            .Operations()
			                            .Start();

			var id = Guid.NewGuid().ToString();

			using var scope   = host.Services.CreateScope();
			var       users   = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var       context = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();
			{
				var create = await users.CreateAsync(new User
				{
					Id          = id,
					UserName    = "SubjectUser"
				});
				create.Succeeded.Should().BeTrue();
				context.Users.Local.Should().NotBeEmpty();
			}

			{
				var subject = scope.ServiceProvider.GetRequiredService<Subject>();

				const string expected = "Hello World! NEW!";
				var source = new ClaimsPrincipal(new ClaimsIdentity(new[]
				{
					new System.Security.Claims.Claim(ClaimTypes.Name, id),
					new System.Security.Claims.Claim(DisplayNameClaim.Default, expected)
				}, "AuthenticationTesting"));
				var user = await context.Users.SingleAsync(x => x.Id == id);
				{
					var principal = await scope.ServiceProvider.GetRequiredService<SignInManager<User>>()
					                           .CreateUserPrincipalAsync(user);
					var synchronized = await subject.Get((new Stored<User>(user, principal), source));
					synchronized.Should().BeTrue();
				}

				var claims = await users.GetClaimsAsync(user);
				claims.Only().Value.Should().Be(expected);
			}
			{
				var set = scope.ServiceProvider.GetRequiredService<ApplicationStorage>().Users;
				set.Local.Should().NotBeEmpty();
				set.Only().Id.Should().Be(id);
			}
		}


		[Fact]
		public async Task VerifyModified()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .Then(x => x.WithIdentity<User>()
			                                        .StoredIn<ApplicationStorage>()
			                                        .Using.Memory()
			                                        .For<Subject>()
			                                        .Register.Scoped())
			                            .As.Is()
			                            .Operations()
			                            .Start();

			var id = Guid.NewGuid().ToString();

			using var scope   = host.Services.CreateScope();
			var       users   = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var       context = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();
			{
				var create = await users.CreateAsync(new User
				{
					Id          = id,
					UserName    = "SubjectUser"
				});
				create.Succeeded.Should().BeTrue();
				context.Users.Local.Should().NotBeEmpty();
			}

			{
				var user = await context.Users.SingleAsync(x => x.Id == id);
				await users.AddClaimAsync(user,
				                          new System.Security.Claims.Claim(DisplayNameClaim.Default, "Hello World!"));
			}


			{
				var subject = scope.ServiceProvider.GetRequiredService<Subject>();

				const string expected = "Hello World! NEW!";
				var source = new ClaimsPrincipal(new ClaimsIdentity(new[]
				{
					new System.Security.Claims.Claim(ClaimTypes.Name, id),
					new System.Security.Claims.Claim(DisplayNameClaim.Default, expected)
				}, "AuthenticationTesting"));
				var user = await context.Users.SingleAsync(x => x.Id == id);
				{
					var principal = await scope.ServiceProvider.GetRequiredService<SignInManager<User>>()
					                           .CreateUserPrincipalAsync(user);
					var synchronized = await subject.Get((new Stored<User>(user, principal), source));
					synchronized.Should().BeTrue();
				}

				var claims = await users.GetClaimsAsync(user);
				claims.Only().Value.Should().Be(expected);
			}
		}


		[Fact]
		public async Task VerifyUnmodified()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .Then(x => x.WithIdentity<User>()
			                                        .StoredIn<ApplicationStorage>()
			                                        .Using.Memory()
			                                        .For<Subject>()
			                                        .Register.Scoped())
			                            .As.Is()
			                            .Operations()
			                            .Start();

			var id = Guid.NewGuid().ToString();

			using var scope   = host.Services.CreateScope();
			var       users   = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var       context = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();
			{
				var create = await users.CreateAsync(new User
				{
					Id          = id,
					UserName    = "SubjectUser"
				});
				create.Succeeded.Should().BeTrue();
				context.Users.Local.Should().NotBeEmpty();
			}

			{
				var user = await context.Users.SingleAsync(x => x.Id == id);
				await users.AddClaimAsync(user,
				                          new System.Security.Claims.Claim(DisplayNameClaim.Default, "Hello World!"));
			}


			{
				var subject = scope.ServiceProvider.GetRequiredService<Subject>();

				const string expected = "Hello World!";
				var source = new ClaimsPrincipal(new ClaimsIdentity(new[]
				{
					new System.Security.Claims.Claim(ClaimTypes.Name, id),
					new System.Security.Claims.Claim(DisplayNameClaim.Default, expected)
				}, "AuthenticationTesting"));
				var user = await context.Users.SingleAsync(x => x.Id == id);
				{
					var principal = await scope.ServiceProvider.GetRequiredService<SignInManager<User>>()
					                           .CreateUserPrincipalAsync(user);
					var synchronized = await subject.Get((new Stored<User>(user, principal), source));
					synchronized.Should().BeFalse();
				}

				var claims = await users.GetClaimsAsync(user);
				claims.Only().Value.Should().Be(expected);
			}
		}

		sealed class Subject : UserSynchronizer<User>
		{
			public Subject(UserManager<User> users)
				: base(users, new Claims(x => x.Type.StartsWith(ClaimNamespace.Default))) {}
		}

		sealed class DisplayNameClaim : Claim
		{
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

		}

		sealed class StorageBuilder : StorageBuilder<ApplicationStorage>
		{
			[UsedImplicitly]
			public static StorageBuilder Default { get; } = new StorageBuilder();

			StorageBuilder() : base(InMemoryConfiguration.Default.Execute) {}
		}
	}
}