using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Runtime;

sealed class IsAssignedConditions<T> : ReferenceValueStore<Type, Func<T, bool>>
{
	readonly static Type Type = AccountForUnassignedType.Default.Get(A.Type<T>());

	[UsedImplicitly]
	public static IsAssignedConditions<T> Default { get; } = new IsAssignedConditions<T>();

	IsAssignedConditions()
		: this(IsAssignableStructure.Default.Get(Type)
			       ? Start.A.Generic(typeof(HasValue<>))
			              .Of.Type<T>()
			              .As.Condition()
			              .Then()
			              .Invoke()
			              .Bind(A.Type<T>().Yield().Result())
			              .Get()
			              .Then()
			              .Assume()
			              .Get()
			       : IsReference.Default.Get(Type)
				       ? Start.A.Selection<T>()
				              .AndOf<object>()
				              .By.Cast.Select(Is.Assigned().Get())
				       : IsModified<T>.Default) {}

	public IsAssignedConditions(ISelect<T, bool> condition)
		: base(Start.A.Selection.Of.System.Type.By.Returning(condition).Get().Then().Delegate()) {}
}