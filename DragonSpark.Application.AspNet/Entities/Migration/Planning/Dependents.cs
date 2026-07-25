using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public sealed class Dependents : Dictionary<List<IEntityType>, HashSet<List<IEntityType>>>;