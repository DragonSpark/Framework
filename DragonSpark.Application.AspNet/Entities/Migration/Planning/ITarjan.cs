using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public interface ITarjan : ISelect<Dictionary<IEntityType, HashSet<IEntityType>>, Entities>;