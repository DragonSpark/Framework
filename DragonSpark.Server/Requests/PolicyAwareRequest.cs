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
		public PolicyAwareRequest(ISelecting<Guid, string?> owner, ISelecting<TIn, TOut?> select,
		                          IRequesting<TIn> other)
			: this(new Policy(owner), select, other) {}

		public PolicyAwareRequest(IPolicy policy, ISelecting<TIn, TOut?> select, IRequesting<TIn> other)
			: base(policy, new SelectedRequest<TIn, TOut>(select), other) {}
	}

	public class PolicyAwareRequest<T> : IRequesting<T>
	{
		readonly IPolicy<T>                            _policy;
		readonly ISelecting<Request<T>, IActionResult> _previous, _other;

		public PolicyAwareRequest(ISelecting<Guid, string?> owner, ISelect<T, ValueTask> select,
		                          ISelect<T, ValueTask> other)
			: this(new Policy(owner), select, new Ok<T>(other)) {}

		public PolicyAwareRequest(IPolicy policy, ISelect<T, ValueTask> select, IRequesting<T> other)
			: this(policy, new Ok<T>(select), other) {}

		public PolicyAwareRequest(ISelecting<Guid, string?> owner,
		                          ISelecting<Request<T>, IActionResult> previous,
		                          ISelecting<Request<T>, IActionResult> other)
			: this(new Policy(owner), previous, other) {}

		public PolicyAwareRequest(IPolicy policy, ISelecting<Request<T>, IActionResult> previous,
		                          ISelecting<Request<T>, IActionResult> other)
			: this(new Policy<T>(policy), previous, other) {}

		public PolicyAwareRequest(IPolicy<T> policy, ISelecting<Request<T>, IActionResult> previous,
		                          ISelecting<Request<T>, IActionResult> other)
		{
			_policy   = policy;
			_previous = previous;
			_other    = other;
		}

		public async ValueTask<IActionResult> Get(Request<T> parameter)
		{
			var (owner, _) = parameter;
			var policy = await _policy.Await(parameter);
			var result = policy.HasValue
				             ? policy.Value
					               ? await _previous.Await(parameter)
					               : await _other.Await(parameter)
				             : owner.NotFound();
			return result;
		}
	}
}