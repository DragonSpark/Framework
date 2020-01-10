using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Activation
{
	sealed class SingletonProperty : ReferenceValueStore<Type, PropertyInfo>
	{
		public static SingletonProperty Default { get; } = new SingletonProperty();

		SingletonProperty() : this(SingletonCandidates.Default) {}

		public SingletonProperty(IResult<Array<string>> candidates)
			: base(Start.A.Selection.Of.System.Type.By.Delegate<string, PropertyInfo>(x => x.GetProperty)
			            .Select(candidates.Select)
			            .Get()
			            .Then()
			            .Value()
			            .Query()
			            .Where(IsSingletonProperty.Default)
			            .FirstAssigned() // TODO: combine condition.
			            .Get) {}
	}
}