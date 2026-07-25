using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IDestination<TFrom, out TTo> : ISelect<Stop<DestinationInput<TFrom>>, IAsyncEnumerable<TTo>>;