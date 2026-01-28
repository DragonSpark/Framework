using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IProcessChanges<T> : IStopAware<ProcessChangesInput<T>, uint>;