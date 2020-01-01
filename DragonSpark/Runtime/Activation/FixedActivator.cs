using System;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Activation
{
	public class FixedActivator<T> : FixedSelection<Type, T>, IActivator<T>, IActivateUsing<ISelect<Type, T>>
	{
		public FixedActivator(ISelect<Type, T> select) : base(select, Type<T>.Instance) {}
	}
}