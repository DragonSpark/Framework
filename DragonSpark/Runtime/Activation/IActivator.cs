using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Activation;

public interface IActivator<out T> : IResult<T> {}

public interface IActivator : ISelect<Type, object> {}