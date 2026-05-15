using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public interface IMigrationSteps : ISelect<Array<IEntityMigrator>, IEnumerable<IMigrationStep>>;