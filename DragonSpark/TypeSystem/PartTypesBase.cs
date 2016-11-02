using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public abstract class PartTypesBase : ParameterizedSourceBase<IEnumerable<Assembly>, IEnumerable<Type>>
	{
		readonly static Func<IEnumerable<Assembly>, Assembly> AssemblySource = ApplicationAssemblyLocator.Default.Get;

		readonly Func<Assembly, ImmutableArray<Type>> typeSource;
		readonly Func<IEnumerable<Assembly>, Assembly> assemblySource;

		protected PartTypesBase( Func<Assembly, ImmutableArray<Type>> typeSource ) : this( typeSource, AssemblySource ) {}

		[UsedImplicitly]
		protected PartTypesBase( Func<Assembly, ImmutableArray<Type>> typeSource, Func<IEnumerable<Assembly>, Assembly> assemblySource )
		{
			this.typeSource = typeSource;
			this.assemblySource = assemblySource;
		}

		public override IEnumerable<Type> Get( IEnumerable<Assembly> parameter )
		{
			var assembly = assemblySource( parameter );
			var types = typeSource( assembly );
			var result = types.AsEnumerable();
			return result;
		}
	}
}