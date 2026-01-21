using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IProcessors<T> : ISelect<ProcessorsInput, IEntityProcessor<T>>;