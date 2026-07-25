using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

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