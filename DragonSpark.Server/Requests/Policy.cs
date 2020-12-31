using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	public class Policy : Selecting<Query, bool?>, IPolicy
	{
		public Policy(ISelecting<Guid, string?> owner) : base(new IsOwner(owner)) {}

		public Policy(ISelect<Query, ValueTask<bool?>> select) : base(select) {}
	}
}