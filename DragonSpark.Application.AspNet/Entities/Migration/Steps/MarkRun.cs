using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class MarkRun : IStopAware<DbContext>
{
	public static MarkRun Default { get; } = new();

	MarkRun()
		: this(ContextName.Default, MigrationRun.Default,
		       """
		       EXEC sys.sp_addextendedproperty 
		              @name = {0},
		              @value = N'1';
		       """) {}

	readonly ISelect<IModel, string> _name;
	readonly string                  _text, _sql;

	public MarkRun(ISelect<IModel, string> name, string text, string sql)
	{
		_name = name;
		_text = text;
		_sql  = sql;
	}

	public async ValueTask Get(Stop<DbContext> parameter)
	{
		var (subject, stop) = parameter;
		var name = _name.Get(subject.Model).Verify();
		await subject.Database.ExecuteSqlRawAsync(_sql, [$"{_text}:{name}"], stop).Off();
	}
}