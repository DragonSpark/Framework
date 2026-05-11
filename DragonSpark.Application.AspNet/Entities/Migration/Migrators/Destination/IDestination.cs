using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IDestination<TFrom, out TTo> : ISelect<Stop<DestinationInput<TFrom>>, IAsyncEnumerable<TTo>>;