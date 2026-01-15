using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public interface ITopologicalSort : IArray<Lease<IEntityType>, IEntityType>;