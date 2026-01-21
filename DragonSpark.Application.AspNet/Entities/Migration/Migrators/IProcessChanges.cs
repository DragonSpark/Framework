using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IProcessChanges<T> : ISelect<ProcessChangesInput<T>, uint>;