using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Model;

public interface IInput<TSubject, TOut> : ISelecting<UserInput<TSubject>, TOut>;