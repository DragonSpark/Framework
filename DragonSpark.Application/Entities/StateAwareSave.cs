using DragonSpark.Compose;
using DragonSpark.Composition;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
    sealed class StateAwareSave<T> : ISave<T> where T : class
	{
		readonly ISave<T>             _previous;
		readonly Func<ExcludeSession> _session;

		[UsedImplicitly]
		public StateAwareSave(DbContext context, ISave<T> previous)
			: this(previous, Excludes.Default.Then().Bind(context)) {}

		[Candidate(false)]
		public StateAwareSave(ISave<T> previous, Func<ExcludeSession> session)
		{
			_previous = previous;
			_session  = session;
		}

		public async ValueTask Get(T parameter)
		{
			await using var session = _session();
			await _previous.Await(parameter);
		}
	}
}