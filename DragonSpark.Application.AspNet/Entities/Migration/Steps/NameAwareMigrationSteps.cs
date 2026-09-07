using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class NameAwareMigrationSteps : IMigrationSteps
{
	readonly IMigrationSteps _previous;
	readonly IMigrationStep  _set;
	readonly IMigrationStep  _mark;

	public NameAwareMigrationSteps(IMigrationSteps previous, string name)
		: this(previous, new SetMigrationName(name), PersistMigrationNameStep.Default) {}

	public NameAwareMigrationSteps(IMigrationSteps previous, IMigrationStep set, IMigrationStep mark)
	{
		_previous = previous;
		_set      = set;
		_mark     = mark;
	}

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
		=> _previous.Get(parameter).Append(_mark).Prepend(_set);
}