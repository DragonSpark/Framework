using DragonSpark.Compose;
using DragonSpark.Composition;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	sealed class StateAwareSave<T> : ISessionSave<T> where T : class
	{
		readonly ISessionSave<T>      _previous;
		readonly Func<ExcludeSession> _session;

		[UsedImplicitly]
		public StateAwareSave(DbContext context, ISessionSave<T> previous)
			: this(previous, Excludes.Default.Then().Bind(context)) {}

		[Candidate(false)]
		public StateAwareSave(ISessionSave<T> previous, Func<ExcludeSession> session)
		{
			_previous = previous;
			_session  = session;
		}

		public async ValueTask Get(T parameter)
		{
			using var session = _session();
			await _previous.Await(parameter);
		}
	}
}