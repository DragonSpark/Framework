using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IRunner<T> : ISelect<Stop<PageInput<T>>, RunnerResult>;