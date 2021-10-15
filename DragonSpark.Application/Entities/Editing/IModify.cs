using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Entities.Editing;

public interface IModify<T> : ICommand<Edit<T>> {}