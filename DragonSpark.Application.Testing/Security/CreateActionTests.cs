using DragonSpark.Application.Hosting.Server.Blazor;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Model;
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
using Claims = DragonSpark.Application.Testing.Objects.Claims;

namespace DragonSpark.Application.Testing.Security
{
	public sealed class CreateActionTests
	{
		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithDefaultComposition()
			                            .WithBlazorServerApplication()
			                            .WithIdentity()
			                            .CreatedWith(NewUser.Default)
			                            .Having(Claims.Default)
			                            .StoredIn<ApplicationStorage>()
			                            .Using.Memory()
			                            .As.Is()
			                            .Operations()
			                            .Run();

			using var scope = host.Services.CreateScope();

			var model = scope.ServiceProvider.GetRequiredService<ICreateAction>();

			var storage = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();

			storage.Users.Should().BeEmpty();

			var          id       = Guid.NewGuid().ToString();
			const string expected = "Hello World! NEW!";
			var          claim    = new System.Security.Claims.Claim(DisplayNameClaim.Default, expected);
			var source = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new System.Security.Claims.Claim(ClaimTypes.Name, id), claim
			}, "AuthenticationTesting"));
			var login   = new ExternalLoginInfo(source, "UnitTesting", id, "Display Name");
			var request = await model.Get(login);

			request.Succeeded.Should().BeTrue();


			var user = await storage.Users.SingleAsync(x => x.Id == login.UniqueId());
			user.Should().NotBeNull();

			var users  = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var claims = await users.GetClaimsAsync(user);
			claims.Only().Should().BeEquivalentTo(claim);

			var actual = (await users.GetLoginsAsync(user)).Only();
			actual.LoginProvider.Should().Be(login.LoginProvider);
			actual.ProviderKey.Should().Be(login.ProviderKey);
		}

		[Fact]
		public async Task VerifyExisting()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithDefaultComposition()
			                            .WithBlazorServerApplication()
			                            .WithIdentity()
			                            .CreatedWith(NewUser.Default)
			                            .Having(Claims.Default)
			                            .StoredIn<ApplicationStorage>()
			                            .Using.Memory()
			                            .Then(x => x.AddSingleton<IExternalSignin>(ExternalSignIn.Default))
			                            .As.Is()
			                            .Operations()
			                            .Run();

			using var scope = host.Services.CreateScope();

			var model   = scope.ServiceProvider.GetRequiredService<ExternalLoginModelActions<User>>();
			var storage = scope.ServiceProvider.GetRequiredService<ApplicationStorage>();

			var id = Guid.NewGuid().ToString();

			const string expected = "Hello World! NEW!";
			var          claim    = new System.Security.Claims.Claim(DisplayNameClaim.Default, expected);
			var source = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new System.Security.Claims.Claim(ClaimTypes.Name, id), claim
			}, "AuthenticationTesting"));
			var login = new ExternalLoginInfo(source, "UnitTesting", id, "Display Name");
			var users = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			{
				var @new = new User
				{
					Id       = login.UniqueId(),
					UserName = "SubjectUser"
				};
				var create = await users.CreateAsync(@new);
				create.Succeeded.Should().BeTrue();
				await users.AddClaimAsync(@new,
				                          new System.Security.Claims.Claim(DisplayNameClaim.Default, "Hello World!"));
			}

			{
				var user = await storage.Users.SingleAsync(x => x.Id == login.UniqueId());
				user.Should().NotBeNull();

				var claims = await users.GetClaimsAsync(user);
				claims.Only()
				      .Should()
				      .BeEquivalentTo(new System.Security.Claims.Claim(DisplayNameClaim.Default, "Hello World!"));
			}

			var request = await model.Get(new CallbackContext(login, "/"));

			{
				request.Should().NotBeNull();

				var user = await storage.Users.SingleAsync(x => x.Id == login.UniqueId());
				user.Should().NotBeNull();

				var claims = await users.GetClaimsAsync(user);
				claims.Only().Should().BeEquivalentTo(claim);
			}
		}
	}
}