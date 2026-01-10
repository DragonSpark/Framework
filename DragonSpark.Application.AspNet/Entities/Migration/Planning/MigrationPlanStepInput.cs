using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct MigrationPlanStepInput(
	IForeignKey Key,
	ITable<IEntityType, HashSet<IEntityType>> Graph,
	IEntityType Type,
	IDictionary<IEntityType, HashSet<IEntityType>> References);