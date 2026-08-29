using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public interface ITarjan : ISelect<Dictionary<IEntityType, List<IEntityType>>, Entities>;