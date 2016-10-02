using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public abstract class PartTypesBase : ParameterizedSourceBase<IEnumerable<Assembly>, IEnumerable<Type>>
	{
		readonly Func<Assembly, ImmutableArray<Type>> typeSource;
		readonly Func<IEnumerable<Assembly>, Assembly> assemblySource;

		protected PartTypesBase( Func<Assembly, ImmutableArray<Type>> typeSource ) : this( typeSource, ApplicationAssemblyLocator.Default.Get ) {}

		protected PartTypesBase( Func<Assembly, ImmutableArray<Type>> typeSource, Func<IEnumerable<Assembly>, Assembly> assemblySource )
		{
			this.typeSource = typeSource;
			this.assemblySource = assemblySource;
		}

		public override IEnumerable<Type> Get( IEnumerable<Assembly> parameter ) => typeSource( assemblySource( parameter ) ).AsEnumerable();
	}
}