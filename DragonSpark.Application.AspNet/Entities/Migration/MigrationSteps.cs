using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class MigrationSteps : IMigrationSteps
{
	public static MigrationSteps Default { get; } = new();

	MigrationSteps() {}

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
	{
		yield return new PreMigrationStep(parameter);
		yield return new MigrationStep(parameter);
		yield return new PostMigrationStep(parameter);
	}
}

// TODO V2

sealed class MarkedAwareMigrationSteps : IMigrationSteps
{
	readonly IMigrationSteps _previous;
	readonly IMigrationStep  _set;
	readonly IMigrationStep  _mark;

	public MarkedAwareMigrationSteps(IMigrationSteps previous, DbContext destination, string name)
		: this(previous, new SetMigrationName(destination, name), new PersistMigrationNameStep(destination)) {}

	public MarkedAwareMigrationSteps(IMigrationSteps previous, IMigrationStep set, IMigrationStep mark)
	{
		_previous = previous;
		_set      = set;
		_mark     = mark;
	}

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
		=> _previous.Get(parameter).Append(_mark).Prepend(_set);
}

sealed class ContextName : ReferenceValueTable<DbContext, string>
{
	public static ContextName Default { get; } = new();

	ContextName() {}
}

sealed class MigrationHasRun : IStopAware<DbContext, bool?>
{
	public static MigrationHasRun Default { get; } = new();

	MigrationHasRun()
		: this(ContextName.Default, MigrationRun.Default,
		       """
		       SELECT 1 
		           WHERE EXISTS (
		               SELECT * 
		               FROM sys.extended_properties 
		               WHERE name = {0}
		           );
		       """) {}

	readonly ISelect<DbContext, string> _name;
	readonly string                     _text;
	readonly string                     _sql;

	public MigrationHasRun(ISelect<DbContext, string> name, string text, string sql)
	{
		_name = name;
		_text = text;
		_sql  = sql;
	}

	public async ValueTask<bool?> Get(Stop<DbContext> parameter)
	{
		var (subject, stop) = parameter;
		var name = _name.Get(subject);
		if (name.IsAssigned())
		{
			var rows   = await subject.Database.SqlQueryRaw<int>(_sql, [$"{_text}:{name}"]).ToArrayAsync(stop).Off();
			var result = rows.Length == 1;
			return result;
		}

		return null;
	}
}

sealed class MigrationRun : Text.Text
{
	public static MigrationRun Default { get; } = new();

	MigrationRun() : base(A.Type<MigrationRun>().FullName.Verify()) {}
}

sealed class MarkRun : IStopAware<DbContext>
{
	public static MarkRun Default { get; } = new();

	MarkRun() : this(ContextName.Default, MigrationRun.Default,
	                 """

	                         EXEC sys.sp_addextendedproperty 
	                             @name = {0},
	                             @value = N'1';
	                     
	                 """) {}

	readonly ISelect<DbContext, string> _name;
	readonly string                     _text;
	readonly string                     _sql;

	public MarkRun(ISelect<DbContext, string> name, string text, string sql)
	{
		_name = name;
		_text = text;
		_sql  = sql;
	}

	public async ValueTask Get(Stop<DbContext> parameter)
	{
		var (subject, stop) = parameter;
		var name = _name.Get(subject).Verify();
		await subject.Database.ExecuteSqlRawAsync(_sql, [$"{_text}:{name}"], stop).Off();
	}
}

sealed class SetMigrationName : IMigrationStep
{
	readonly DbContext                 _subject;
	readonly string                    _name;
	readonly ITable<DbContext, string> _store;

	public SetMigrationName(DbContext subject, string name) : this(subject, name, ContextName.Default) {}

	public SetMigrationName(DbContext subject, string name, ITable<DbContext, string> store)
	{
		_subject = subject;
		_name    = name;
		_store   = store;
	}

	public ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		_store.Assign(_subject, _name);
		return ValueTask.CompletedTask;
	}
}

sealed class PersistMigrationNameStep : IMigrationStep
{
	readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<bool?> _run;
	readonly IStopAware                                                  _mark;

	public PersistMigrationNameStep(DbContext context)
		: this(MigrationHasRun.Default.Then().Bind(context).Out(), MarkRun.Default.Then().Bind(context).Out()) {}

	public PersistMigrationNameStep(DragonSpark.Model.Operations.Results.Stop.IStopAware<bool?> run, IStopAware mark)
	{
		_run  = run;
		_mark = mark;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var (_, stop) = parameter;

		var run = await _run.Off(stop);
		if (run is not null && !run.Value)
		{
			await _mark.Off(stop);
		}
	}
}