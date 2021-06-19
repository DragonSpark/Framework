using DragonSpark.Application;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Claims;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Security.Identity
{
	public sealed class CurrentAuthenticationMethod : Result<string>
	{
		public CurrentAuthenticationMethod(ICurrentPrincipal source) : this(source, AuthenticationMethod.Default) {}

		public CurrentAuthenticationMethod(ICurrentPrincipal source, IReadClaim read)
			: base(source.Then().Select(read).Select(x => x.Value())) {}
	}
}