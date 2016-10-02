using System;
using System.Composition;
using System.Reflection;
using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Composition
{
	sealed class AppliedExportLocator : ParameterizedSourceBase<Type, AppliedExport>
	{
		public static IParameterizedSource<Type, AppliedExport> Default { get; } = new AppliedExportLocator().ToCache();
		AppliedExportLocator() {}

		public override AppliedExport Get( Type parameter )
		{
			foreach ( var candidate in parameter.GetTypeInfo().Append<MemberInfo>( SingletonProperties.Default.Get( parameter ) ).WhereAssigned() )
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