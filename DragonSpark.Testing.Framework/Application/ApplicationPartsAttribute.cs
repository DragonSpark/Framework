using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public class ApplicationPartsAttribute : PartsAttributeBase
	{
		// public ApplicationPartsAttribute() : this( m => {} ) {}
		protected ApplicationPartsAttribute( Action<MethodBase> initialize ) : base( m => AllParts.Default.Get( m.DeclaringType.Assembly ), initialize ) {}
	}

	public abstract class PartsAttributeBase : TypeProviderAttributeBase
	{
		/*protected PartsAttributeBase( Func<MethodBase, ImmutableArray<Type>> factory ) : this( factory, m => {} ) {}*/
		protected PartsAttributeBase( Func<MethodBase, ImmutableArray<Type>> factory, Action<MethodBase> initialize ) : base( new ConfiguringFactory<MethodBase, ImmutableArray<Type>>( factory, initialize ).Get ) {}
	}
}