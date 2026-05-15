using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public sealed class Dependents : Dictionary<List<IEntityType>, HashSet<List<IEntityType>>>;