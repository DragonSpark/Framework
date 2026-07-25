using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public interface IComposeGraph : ISelect<Lease<IEntityType>, Dictionary<IEntityType, HashSet<IEntityType>>>;