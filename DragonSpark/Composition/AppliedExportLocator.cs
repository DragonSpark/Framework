using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Composition;
using System.Reflection;

namespace DragonSpark.Composition
{
	sealed class AppliedExportLocator : CacheWithImplementedFactoryBase<Type, AppliedExport>
	{
		readonly Func<Type, PropertyInfo> propertySource;
		public static IParameterizedSource<Type, AppliedExport> Default { get; } = new AppliedExportLocator();
		AppliedExportLocator() : this( SingletonProperties.Default.Get ) {}

		[UsedImplicitly]
		public AppliedExportLocator( Func<Type, PropertyInfo> propertySource )
		{
			this.propertySource = propertySource;
		}

		protected override AppliedExport Create( Type parameter )
		{
			foreach ( var candidate in parameter.GetTypeInfo().Append<MemberInfo>( propertySource( parameter ) ).WhereAssigned() )
			{
				var attribute = candidate.GetAttribute<ExportAttribute>();
				if ( attribute != null )
				{
					var result = new AppliedExport( parameter, candidate, attribute.ContractType ?? candidate.GetMemberType() );
					return result;
				}
			}

			return default(AppliedExport);
		}
	}
}