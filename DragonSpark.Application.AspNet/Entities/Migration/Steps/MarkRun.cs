using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

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