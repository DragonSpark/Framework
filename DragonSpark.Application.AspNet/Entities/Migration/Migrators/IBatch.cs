using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IBatch<T> : ICommand<BatchInput<T>>;