using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class SignOutScheme : IOperation
{
	readonly ICurrentContext _accessor;
	readonly string          _scheme;

	public SignOutScheme(ICurrentContext accessor) : this(accessor, IdentityConstants.ExternalScheme) {}

	public SignOutScheme(ICurrentContext accessor, string scheme)
	{
		_accessor = accessor;
		_scheme   = scheme;
	}

	public ValueTask Get() => _accessor.Get().SignOutAsync(_scheme).ToOperation();
}