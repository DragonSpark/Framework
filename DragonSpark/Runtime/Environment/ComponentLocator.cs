using System;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using Activator = DragonSpark.Runtime.Activation.Activator;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentLocator<T> : FixedActivator<T>, IActivateUsing<ISelect<Type, Type>>
	{
		public static ComponentLocator<T> Default { get; } = new ComponentLocator<T>();

		ComponentLocator() : this(ComponentType.Default) {}

		public ComponentLocator(ISelect<Type, Type> type)
			: base(type.Select(Activator.Default.Assigned()).Then().CastForResult<T>().Get()) {}
	}
}