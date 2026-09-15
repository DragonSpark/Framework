using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class NameAwareMigrationSteps : IMigrationSteps
{
	readonly IMigrationSteps _previous;
	readonly IMigrationStep  _name;
	readonly IMigrationStep  _mark;

	public NameAwareMigrationSteps(IMigrationSteps previous, string name)
		: this(previous, new SetMigrationName(name), PersistMigrationNameStep.Default) {}

	public NameAwareMigrationSteps(IMigrationSteps previous, IMigrationStep name, IMigrationStep mark)
	{
		_previous = previous;
		_name      = name;
		_mark     = mark;
	}

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
		=> _previous.Get(parameter).Prepend(_name).Append(_mark);
}