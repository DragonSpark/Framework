using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests;

public sealed class IsOwner : IIsOwner
{
	readonly IStopAware<Guid, uint?>  _owner;
	readonly IEqualityComparer<uint?> _equals;

	public IsOwner(IStopAware<Guid, uint?> owner) : this(owner, EqualityComparer<uint?>.Default) {}

	public IsOwner(IStopAware<Guid, uint?> owner, IEqualityComparer<uint?> equals)
	{
		_owner  = owner;
		_equals = @equals;
	}

	public async ValueTask<bool?> Get(Stop<Unique> parameter)
	{
		var ((user, identity), stop) = parameter;
		var owner  = await _owner.Off(new(identity, stop));
		var result = owner != null ? _equals.Equals(owner, user) : (bool?)null;
		return result;
	}
}