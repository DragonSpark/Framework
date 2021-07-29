using DragonSpark.Model.Selection;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims
{
	public interface IAccessClaim<T> : ISelect<ClaimsPrincipal, Claim<T>> {}

	public class AccessClaim<T> : IAccessClaim<T>
	{
		readonly IReadClaim      _read;
		readonly Func<string, T> _select;
		readonly Claim<T>        _default;

		public AccessClaim(string claim, Func<string, T> select)
			: this(new ReadClaim(claim), select, Claim<T>.Default) {}

		public AccessClaim(IReadClaim read, Func<string, T> select, Claim<T> @default)
		{
			_read    = read;
			_select  = @select;
			_default = @default;
		}

		public Claim<T> Get(ClaimsPrincipal parameter)
		{
			var (name, exists, value) = _read.Get(parameter);
			var result = exists
				             ? new Claim<T>(_select(value ??
				                                    throw new
					                                    InvalidOperationException($"Claim '{name}' value exists but its value is invalid/null.")))
				             : _default;
			return result;
		}
	}
}