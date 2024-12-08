using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Messaging;

public interface IMessageTemplate<in T> : ISelecting<T, Message>;