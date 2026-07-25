using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Connections.Events;

public interface ISubscribe<T> : ISelect<Func<Stop<T>, Task>, ISubscription>;