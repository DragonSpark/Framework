using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation
{
	public sealed class ConstructingParameterLocator : ParameterizedSourceBase<Type, Type>
	{
		public static ConstructingParameterLocator Default { get; } = new ConstructingParameterLocator();
		ConstructingParameterLocator() {}

		public override Type Get( Type parameter ) => 
			InstanceConstructors.Default.Get( parameter.GetTypeInfo() ).Select( info => info.GetParameterTypes().ToArray() ).SingleOrDefault( types => types.Length == 1 )?.Single();
	}
}