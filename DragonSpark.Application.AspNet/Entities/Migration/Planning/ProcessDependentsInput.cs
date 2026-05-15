using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct ProcessDependentsInput(
	StandardTable<IEntityType, HashSet<IEntityType>> graph,
	Dictionary<IEntityType, HashSet<IEntityType>> indegree);