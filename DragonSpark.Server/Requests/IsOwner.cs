using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	public class IsOwner : IIsOwner
	{
		readonly ISelecting<Guid, string?> _owner;
		readonly IEqualityComparer<string> _equals;

		public IsOwner(ISelecting<Guid, string?> owner) : this(owner, StringComparer.InvariantCultureIgnoreCase) {}

		public IsOwner(ISelecting<Guid, string?> owner, IEqualityComparer<string> equals)
		{
			_owner  = owner;
			_equals = @equals;
		}

		public async ValueTask<bool?> Get(Unique parameter)
		{
			var (user, identity) = parameter;
			var owner  = await _owner.Await(identity);
			var result = owner != null ? _equals.Equals(owner, user) : (bool?)null;
			return result;
		}
	}
}