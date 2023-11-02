using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Messaging;

public interface IMessageTemplate<in T> : ISelecting<T, Message>;