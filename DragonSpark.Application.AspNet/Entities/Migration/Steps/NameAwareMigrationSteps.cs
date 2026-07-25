using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class NameAwareMigrationSteps : IMigrationSteps
{
	readonly IMigrationSteps _previous;
	readonly IMigrationStep  _set;
	readonly IMigrationStep  _mark;

	public NameAwareMigrationSteps(IMigrationSteps previous, DbContext destination, string name)
		: this(previous, new SetMigrationName(destination, name), new PersistMigrationNameStep(destination)) {}

	public NameAwareMigrationSteps(IMigrationSteps previous, IMigrationStep set, IMigrationStep mark)
	{
		_previous = previous;
		_set      = set;
		_mark     = mark;
	}

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
		=> _previous.Get(parameter).Append(_mark).Prepend(_set);
}