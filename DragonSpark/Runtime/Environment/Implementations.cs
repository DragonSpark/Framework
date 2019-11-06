using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Environment
{
	static class Implementations
	{
		public static ISelect<Type, object> Activator { get; }
			= Start.A.Selection.Of.System.Type.By.Array()
			       .Select(Start.A.Selection<Type>()
			                    .By.StoredActivation<MakeGenericType>()
			                    .In(StorageTypeDefinition.Default)
			                    .Assume())
			       .Select(Activation.Activator.Default);
	}

	static class Implementations<T>
	{
		public static IResult<IStore<T>> Store { get; } = Start.A.Selection<IMutable<T>>()
		                                                       .By.StoredActivation<Store<T>>()
		                                                       .In(SystemStores<T>.Default);
	}
}