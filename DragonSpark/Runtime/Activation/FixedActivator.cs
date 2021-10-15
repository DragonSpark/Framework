using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Activation;

public class FixedActivator<T> : FixedSelection<Type, T>, IActivator<T>, IActivateUsing<ISelect<Type, T>>
{
	public FixedActivator(Func<Type, T> select) : base(select, A.Type<T>()) {}
}