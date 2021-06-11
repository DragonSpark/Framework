using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	class Class1 {}

	public sealed class ExternalIdentityValue : RequiredClaim
	{
		public static ExternalIdentityValue Default { get; } = new ExternalIdentityValue();

		ExternalIdentityValue() : base(ExternalIdentity.Default) {}
	}

	public interface IClaim : ISelect<ClaimsPrincipal, string?> {}

	public class ClaimValue : IClaim
	{
		readonly string _claim;

		public ClaimValue(string claim) => _claim = claim;

		public string? Get(ClaimsPrincipal parameter) => parameter.FindFirstValue(_claim);
	}

	public sealed class AuthenticationMethod : ClaimValue
	{
		public static AuthenticationMethod Default { get; } = new AuthenticationMethod();

		AuthenticationMethod() : base(ClaimTypes.AuthenticationMethod) {}
	}

	public interface IRequiredClaim : ISelect<ClaimsPrincipal, string> {}

	public class RequiredClaim : IRequiredClaim
	{
		readonly string _claim;

		public RequiredClaim(string claim) => _claim = claim;

		public string Get(ClaimsPrincipal parameter)
			=> parameter.FindFirstValue(_claim) ??
			   throw new
				   InvalidOperationException($"Content not found for claim '{_claim}' in user {parameter.UserName()}.");
	}



	public sealed class Identities : Select<ClaimsPrincipal, ProviderIdentity>
	{
		public static Identities Default { get; } = new Identities();

		Identities() : this(ExternalIdentityValue.Default, IdentityParser.Default) {}

		public Identities(IRequiredClaim identity, ISelect<string, ProviderIdentity> parser)
			: base(identity.Then().Select(parser)) {}
	}
}