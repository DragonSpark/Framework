using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Events;

public interface ISubscribe<out T> : ISelect<Func<T, Task>, ISubscription>;