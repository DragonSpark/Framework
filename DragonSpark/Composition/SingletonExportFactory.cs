using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition
{
	sealed class SingletonExportFactory : SingletonDelegates<SingletonExport?>
	{
		public static SingletonExportFactory Default { get; } = new SingletonExportFactory();
		SingletonExportFactory() : base( SingletonProperties.Default.Get, Factory.Implementation.Get ) {}

		sealed class Factory : ParameterizedSourceBase<PropertyInfo, SingletonExport?>
		{
			public static Factory Implementation { get; } = new Factory();
			Factory() : this( SingletonPropertyDelegates.Default.Get, SourceAccountedTypes.Default.Get ) {}

			readonly Func<PropertyInfo, Func<object>> factorySource;
			readonly Func<Type, ImmutableArray<Type>> typesSource;

			[UsedImplicitly]
			public Factory( Func<PropertyInfo, Func<object>> factorySource, Func<Type, ImmutableArray<Type>> typesSource )
			{
				this.factorySource = factorySource;
				this.typesSource = typesSource;
			}

			public override SingletonExport? Get( PropertyInfo parameter )
			{
				var factory = factorySource( parameter );
				if ( factory != null )
				{
					var types = typesSource( parameter.PropertyType );
					var contracts = Expand( parameter.GetCustomAttributes<ExportAttribute>().ToImmutableArray(), types ).Distinct().ToImmutableArray();
					var result = new SingletonExport( parameter, contracts, factory );
					return result;
				}
				return null;
			}

			static IEnumerable<CompositionContract> Expand( ImmutableArray<ExportAttribute> attributes, ImmutableArray<Type> types )
			{
				foreach ( var type in types )
				{
					foreach ( var attribute in attributes )
					{
						yield return new CompositionContract( attribute.ContractType ?? type, attribute.ContractName );
					}
					yield return new CompositionContract( type );
				}
			}
		}
	}
}