using DragonSpark.Model.Operations.Selection.Stop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests;

public class Policy : StopAware<Unique, bool?>, IPolicy
{
	public Policy(IStopAware<Guid, uint?> owner) : base(new IsOwner(owner)) {}

	public Policy(IStopAware<Unique, bool?> select) : base(select) {}
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
		=> _policy.Get(new(new(parameter.Parameter.User, _select(parameter)),
		                   parameter.Owner.HttpContext.RequestAborted));
}