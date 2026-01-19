using DragonSpark.Model.Selection;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IComposeBatch<TFrom, TTo> : ISelect<BatchInput<TFrom>, Lease<TTo>>;