using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class Save : ISave
	{
		readonly DbContext _storage;

		public Save(DbContext storage) => _storage = storage;

		public ValueTask<int> Get() => _storage.SaveChangesAsync().ToOperation();
	}

	sealed class Save<T> : ISave<T> where T : class
	{
		readonly IUpdate<T> _update;
		readonly Operate    _save;

		public Save(DbContext context) : this(new Update<T>(context), new Save(context)) {}

		public Save(IUpdate<T> update, ISave save) : this(update, save.Then().Terminate()) {}

		public Save(IUpdate<T> update, Operate save)
		{
			_update = update;
			_save   = save;
		}

		public ValueTask Get(T parameter)
		{
			_update.Execute(parameter);
			return _save();
		}
	}

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