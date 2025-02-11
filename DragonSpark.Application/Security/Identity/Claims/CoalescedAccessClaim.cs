using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims;

public class CoalescedAccessClaim<T> : IAccessClaim<T>
{
	readonly Array<IReadClaim> _read;
	readonly Func<string, T>   _select;
	readonly Read<T>          _default;

	public CoalescedAccessClaim(Array<string> claims, Func<string, T> select)
		: this(claims.Open().AsValueEnumerable().Select(x => new ReadClaim(x)).ToArray(), select,
		       Read<T>.Default) {}

	public CoalescedAccessClaim(Array<IReadClaim> read, Func<string, T> select, Read<T> @default)
	{
		_read    = read;
		_select  = select;
		_default = @default;
	}

	public Read<T> Get(ClaimsPrincipal parameter)
	{
		foreach (var read in _read.Open())
		{
			var (name, exists, value) = read.Get(parameter);
			if (exists)
			{
				return new(_select(value ??
				                   throw new
					                   InvalidOperationException($"Claim '{name}' value exists but its value is invalid/null.")));
			}
		}

		return _default;
	}
}