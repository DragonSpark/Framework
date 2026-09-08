using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IDestination<T> : ISelect<Stop<DestinationInput<T>>, IAsyncEnumerable<DbContext>>;