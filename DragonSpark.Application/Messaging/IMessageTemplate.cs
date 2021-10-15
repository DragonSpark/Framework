using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Messaging;

public interface IMessageTemplate<in T> : ISelecting<T, Message> {}