using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	public class Policy : Selecting<Unique, bool?>, IPolicy
	{
		public Policy(ISelecting<Guid, string?> owner) : base(new IsOwner(owner)) {}

		public Policy(ISelect<Unique, ValueTask<bool?>> select) : base(select) {}
	}

	public class Policy<T> : IPolicy<T>
	{
		readonly IPolicy                _policy;
		readonly Func<Request<T>, Guid> _select;

		public Policy(IPolicy policy) : this(policy, x => x.Parameter.Identity) {}

		public Policy(IPolicy policy, Func<Request<T>, Guid> select)
		{
			_policy = policy;
			_select = @select;
		}

		public ValueTask<bool?> Get(Request<T> parameter)
			=> _policy.Get(new Unique(parameter.Parameter.UserName, _select(parameter)));
	}
}