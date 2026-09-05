using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public interface IMigrationSteps : IYield<Array<IEntityMigrator>, IMigrationStep>;