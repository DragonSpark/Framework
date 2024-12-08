using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IModify<T> : ICommand<Edit<T>>;