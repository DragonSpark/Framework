using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

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