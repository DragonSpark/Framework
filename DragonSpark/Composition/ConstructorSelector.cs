using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;

namespace DragonSpark.Composition
{
	public sealed class ConstructorSelector : ParameterizedSourceBase<Type, ConstructorInfo>
	{
		readonly Func<ConstructorInfo, bool> specification;

		public ConstructorSelector( Func<ConstructorInfo, bool> specification )
		{
			this.specification = specification;
		}

		public override ConstructorInfo Get( Type parameter ) => 
			InstanceConstructors.Default.Get( parameter.GetTypeInfo() ).AsEnumerable().OrderByDescending( info => info.GetParameters().Length ).FirstOrDefault( specification );
	}
}