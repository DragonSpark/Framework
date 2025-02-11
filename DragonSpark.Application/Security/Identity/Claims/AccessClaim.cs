using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims;

public class AccessClaim : AccessClaim<string>
{
	public AccessClaim(string claim) : base(claim, s => s) {}

	public AccessClaim(IReadClaim read, Read<string> @default) : base(read, s => s, @default) {}
}

public class AccessClaim<T> : IAccessClaim<T>
{
	readonly IReadClaim      _read;
	readonly Func<string, T> _select;
	readonly Read<T>        _default;

	public AccessClaim(string claim, Func<string, T> select) : this(new ReadClaim(claim), select, Read<T>.Default) {}

	public AccessClaim(IReadClaim read, Func<string, T> select, Read<T> @default)
	{
		_read    = read;
		_select  = select;
		_default = @default;
	}

	public Read<T> Get(ClaimsPrincipal parameter)
	{
		var (name, exists, value) = _read.Get(parameter);
		var result = exists
			             ? new(_select(value ??
			                           throw new
				                           InvalidOperationException($"Claim '{name}' value exists but its value is invalid/null.")))
			             : _default;
		return result;
	}
}