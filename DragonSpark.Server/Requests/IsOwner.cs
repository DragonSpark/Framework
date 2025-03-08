using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests;

public sealed class IsOwner : IIsOwner
{
	readonly ISelecting<Guid, uint?> _owner;
	readonly IEqualityComparer<uint?> _equals;

	public IsOwner(ISelecting<Guid, uint?> owner) : this(owner, EqualityComparer<uint?>.Default) {}

	public IsOwner(ISelecting<Guid, uint?> owner, IEqualityComparer<uint?> equals)
	{
		_owner  = owner;
		_equals = @equals;
	}

	public async ValueTask<bool?> Get(Unique parameter)
	{
		var (user, identity) = parameter;
		var owner  = await _owner.Off(identity);
		var result = owner != null ? _equals.Equals(owner, user) : (bool?)null;
		return result;
	}
}