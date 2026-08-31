using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

class ExecuteStepBase : IMigrationStep
{
	readonly IStopAware<ConstraintInput> _input;
	readonly IExecution<ConstraintInput> _execute;

	protected ExecuteStepBase(DatabaseFacade database, ISelect<ConstraintInput, IEnumerable<string>> statements)
		: this(ConstraintInputs.Default.Then().Bind(database).Out(),
		       new Execution<ConstraintInput>(database, statements)) {}

	protected ExecuteStepBase(IStopAware<ConstraintInput> input, IExecution<ConstraintInput> execute)
	{
		_input   = input;
		_execute = execute;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var (_, stop) = parameter;
		var input = await _input.Off(stop);
		await _execute.Off(new(input, stop));
	}
}