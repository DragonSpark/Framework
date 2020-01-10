using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Activation
{
	sealed class Activator : Select<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(Start.A.Generic(typeof(Activator<>))
		                        .Of.Type<object>()
		                        .As.Result()
		                        .Then()
		                        .Invoke()
		                        .To(x => x.Get().Then())
		                        .Value()
		                        .Get()
		                        .To(Start.A.Selection.Of.System.Type.By.Array().Select)) {}
	}

	sealed class Activator<T> : Result<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(New<T>.Default.Then().Unless(Singleton<T>.Default).IsAssigned()) {}
	}
}