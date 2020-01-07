﻿using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace DragonSpark.Runtime
{
	sealed class IsAssignedConditions<T> : ReferenceValueStore<TypeInfo, Func<T, bool>>
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
				              .Assume()
				       : IsReference.Default.Get(Type)
					       ? Start.A.Selection<T>()
					              .AndOf<object>()
					              .By.Cast.Select(Is.Assigned())
					       : IsModified<T>.Default) {}

		public IsAssignedConditions(ISelect<T, bool> condition)
			: base(Start.A.Selection.Of.System.Metadata.By.Returning(condition).Then().Delegate()) {}
	}
}