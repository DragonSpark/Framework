using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public interface ISubscribe : ISelect<Func<Task>, ISubscription> {}

public interface ISubscribe<out T> : ISelect<Func<T, Task>, ISubscription> {}