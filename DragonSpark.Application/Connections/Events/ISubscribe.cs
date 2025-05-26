using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Events;

public interface ISubscribe<T> : ISelect<Func<Stop<T>, Task>, ISubscription>;