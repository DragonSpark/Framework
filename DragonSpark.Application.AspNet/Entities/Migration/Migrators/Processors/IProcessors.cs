using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

public interface IProcessors<T> : ISelect<ProcessorsInput<T>, IEntityProcessor<T>> where T : class;