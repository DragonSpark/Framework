using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Presentation {
	sealed class Properties : IProperties
	{
		public static Properties Default { get; } = new Properties();

		Properties() : this(Operations.Default.Then().Subject.Stores().Reference().Get) {}

		readonly Func<Type, Array<Func<ComponentBase, IOperation>>> _delegates;

		public Properties(Func<Type, Array<Func<ComponentBase, IOperation>>> delegates) => _delegates = delegates;

		public Array<Func<ComponentBase, IOperation>> Get(Type parameter) => _delegates(parameter);
	}
}