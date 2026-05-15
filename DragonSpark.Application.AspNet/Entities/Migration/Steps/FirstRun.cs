using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed class FirstRun : IStopAware<DbContext, bool>
{
	public static FirstRun Default { get; } = new();

	FirstRun() : this(MigrationHasRun.Default) {}
	
	readonly IStopAware<DbContext, bool?> _run;

	public FirstRun(IStopAware<DbContext, bool?> run) => _run = run;

	public async ValueTask<bool> Get(Stop<DbContext> parameter)
	{
		var run = await _run.Off(parameter);
		return run is not null && !run.Value;
	}
}