using DragonSpark.Application.Hosting.Server.Blazor;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Testing.Objects;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Testing.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using ClaimNamespace = DragonSpark.Application.Testing.Objects.ClaimNamespace;
using Claims = DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Testing.Security.Identity
{
	public sealed class UserSynchronizerTests
	{
		[Fact]
		public async Task VerifyAdd()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithBlazorServerApplication()
			                            .WithIdentity()
			                            .Using<User>()
			                            .Having(Objects.Claims.Default)
			                            .StoredIn<ApplicationStorage>()
			                            .Using.Memory()
			                            .Then(x => x.For<Subject>().Register.Scoped())
			                            .As.Is()
			                            .Operations()
			                            .Run();

			var id = Guid.NewGuid().ToString();
			var uniqueId = new ExternalLoginInfo(new ClaimsPrincipal(), "UnitTesting", id, "Display Name").UniqueId();

			using var scope   = host.Services.CreateScope();
			var       users   = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var       context = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();
			{

				var create = await users.CreateAsync(new User
				{
					UserName = uniqueId
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
				var user = await context.Users.SingleAsync(x => x.UserName == uniqueId);
				{
					var principal = await scope.ServiceProvider.GetRequiredService<SignInManager<User>>()
					                           .CreateUserPrincipalAsync(user);
					var synchronization = new Synchronization<User>(new ExternalLoginInfo(principal, string.Empty,
					                                                                      string.Empty, string.Empty),
					                                                new Profile<User>(principal, user), source);
					var synchronized = await subject.Get(synchronization);
					synchronized.Should().BeTrue();
				}

				var claims = await users.GetClaimsAsync(user);
				claims.Only().Value.Should().Be(expected);
			}
			{
				var set = scope.ServiceProvider.GetRequiredService<ApplicationStorage>().Users;
				set.Local.Should().NotBeEmpty();
				var only = set.Only();
				only.UserName.Should().Be(uniqueId);
				only.Modified.Should().NotBeNull();
			}
		}

		[Fact]
		public async Task VerifyModified()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithBlazorServerApplication()
			                            .WithIdentity()
			                            .Using<User>()
			                            .Having(Objects.Claims.Default)
			                            .StoredIn<ApplicationStorage>()
			                            .Using.Memory()
			                            .Then(x => x.For<Subject>().Register.Scoped())
			                            .As.Is()
			                            .Operations()
			                            .Run();

			var id = Guid.NewGuid().ToString();
			var uniqueId = new ExternalLoginInfo(new ClaimsPrincipal(), "UnitTesting", id, "Display Name").UniqueId();

			using var scope   = host.Services.CreateScope();
			var       users   = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var       context = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();
			{
				var create = await users.CreateAsync(new User
				{
					UserName = uniqueId
				});
				create.Succeeded.Should().BeTrue();
				context.Users.Local.Should().NotBeEmpty();
			}

			{
				var user = await context.Users.SingleAsync(x => x.UserName == uniqueId);
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
				var user = await context.Users.SingleAsync(x => x.UserName == uniqueId);
				{
					var principal = await scope.ServiceProvider.GetRequiredService<SignInManager<User>>()
					                           .CreateUserPrincipalAsync(user);
					var synchronization = new Synchronization<User>(new ExternalLoginInfo(principal, string.Empty,
					                                                                      string.Empty, string.Empty),
					                                                new Profile<User>(principal, user), source);
					var synchronized = await subject.Get(synchronization);
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
			                            .WithBlazorServerApplication()
			                            .WithIdentity()
			                            .Using<User>()
			                            .Having(Objects.Claims.Default)
			                            .StoredIn<ApplicationStorage>()
			                            .Using.Memory()
			                            .Then(x => x.For<Subject>().Register.Scoped())
			                            .As.Is()
			                            .Operations()
			                            .Run();

			var id = Guid.NewGuid().ToString();
			var uniqueId = new ExternalLoginInfo(new ClaimsPrincipal(), "UnitTesting", id, "Display Name").UniqueId();

			using var scope   = host.Services.CreateScope();
			var       users   = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var       context = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();
			{
				var create = await users.CreateAsync(new User
				{
					UserName = uniqueId
				});
				create.Succeeded.Should().BeTrue();
				context.Users.Local.Should().NotBeEmpty();
			}

			{
				var user = await context.Users.SingleAsync(x => x.UserName == uniqueId);
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
				var user = await context.Users.SingleAsync(x => x.UserName == uniqueId);
				{
					var principal = await scope.ServiceProvider.GetRequiredService<SignInManager<User>>()
					                           .CreateUserPrincipalAsync(user);
					var synchronization = new Synchronization<User>(new ExternalLoginInfo(principal, string.Empty,
					                                                                      string.Empty, string.Empty),
					                                                new Profile<User>(principal, user), source);
					var synchronized = await subject.Get(synchronization);
					synchronized.Should().BeFalse();
				}

				var claims = await users.GetClaimsAsync(user);
				claims.Only().Value.Should().Be(expected);
			}
		}

		sealed class Subject : UserSynchronizer<User>
		{
			public Subject(UserManager<User> users, ApplicationStorage context)
				: base(new UserClaimSynchronizer<User>(new Claims(x => x.Type.StartsWith(ClaimNamespace.Default)),
				                                       new ClaimTransactions<User>(users)),
				       new MarkModified<User>(context)) {}
		}
	}
}