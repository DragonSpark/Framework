using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	public class PolicyAwareRequest<TIn, TOut> : PolicyAwareRequest<TIn> where TOut : class
	{
		public PolicyAwareRequest(ISelecting<Guid, string?> owner, ISelecting<TIn, TOut?> select, IRequested<TIn> other)
			: this(new Policy(owner), select, other) {}

		public PolicyAwareRequest(IPolicy policy, ISelecting<TIn, TOut?> select, IRequested<TIn> other)
			: base(policy, new Requested<TIn, TOut>(select), other) {}
	}

	public class PolicyAwareRequest<T> : IRequested<T>
	{
		readonly IPolicy       _policy;
		readonly IRequested<T> _previous, _other;

		public PolicyAwareRequest(ISelecting<Guid, string?> owner, ISelect<T, ValueTask> select, IRequested<T> other)
			: this(owner, new Requested<T>(select), other) {}

		public PolicyAwareRequest(ISelecting<Guid, string?> owner, IRequested<T> previous, IRequested<T> other)
			: this(new Policy(owner), previous, other) {}

		public PolicyAwareRequest(IPolicy policy, ISelect<T, ValueTask> select, IRequested<T> other)
			: this(policy, new Requested<T>(select), other) {}

		public PolicyAwareRequest(IPolicy policy, IRequested<T> previous, IRequested<T> other)
		{
			_policy   = policy;
			_previous = previous;
			_other    = other;
		}

		public async ValueTask<IActionResult> Get(Request<T> parameter)
		{
			var (owner, (userName, identity, _)) = parameter;
			var policy = await _policy.Await(new Query(userName, identity));
			var result = policy.HasValue
				             ? policy.Value
					               ? await _previous.Await(parameter)
					               : await _other.Await(parameter)
				             : owner.NotFound();
			return result;
		}
	}
}