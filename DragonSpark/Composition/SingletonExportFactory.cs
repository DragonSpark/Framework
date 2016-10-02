using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;
using System.Composition;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition
{
	sealed class SingletonExportFactory : SingletonDelegates<SingletonExport>
	{
		public static SingletonExportFactory Default { get; } = new SingletonExportFactory();
		SingletonExportFactory() : base( SingletonProperties.Default.Get, new Factory().Get ) {}

		sealed class Factory : ParameterizedSourceBase<PropertyInfo, SingletonExport>
		{
			public override SingletonExport Get( PropertyInfo parameter )
			{
				var instance = SingletonDelegateCache.Default.Get( parameter );
				if ( instance != null )
				{
					var contractType = parameter.PropertyType.Adapt().IsGenericOf( typeof(ISource<>), false ) ? ResultTypes.Default.Get( parameter.PropertyType ) : instance.GetMethodInfo().ReturnType;
					var types = parameter.GetCustomAttributes<ExportAttribute>().Introduce( contractType, x => new CompositionContract( x.Item1.ContractType ?? x.Item2, x.Item1.ContractName ) ).Append( new CompositionContract( contractType ) ).Distinct().ToImmutableArray();
					var result = new SingletonExport( parameter, types, instance.Cache() );
					return result;

				}
				return default(SingletonExport);
			}
		}
	}
}