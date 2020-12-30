using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Server.Requests
{
	public class Policy : Selecting<Query, bool?>, IPolicy
	{
		public Policy(ISelecting<Guid, string?> owner) : base(new IsOwner(owner)) {}
	}
}