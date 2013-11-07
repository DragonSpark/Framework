using System;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System.Linq;

namespace DragonSpark.IoC.Configuration
{
	public class InjectionConstructor : InjectionMemberParameterBase
	{
		public override Microsoft.Practices.Unity.InjectionMember Create( IUnityContainer container, Type targetType )
		{
			var constructors = new ReflectionHelper( targetType ).InstanceConstructors.ToArray();

			var items = Parameters.Resolve( targetType ).ToList();
			
			var matcher = new ParameterMatcher( items );
			if ( !constructors.Any( x =>
			{
				var matches = matcher.Matches( x.GetParameters() );
				return matches;
			} ) )
			{
				var candidate = constructors.FirstOrDefault( x => Find( x, items.ToArray() ) );
				if ( candidate != null )
				{
					var parameters = candidate.GetParameters().Skip( items.Count );
					items.AddRange( parameters.Select( x => new InjectionParameter( x.ParameterType, null ) ) );
				}
				else
				{
					throw new InvalidOperationException( string.Format( "Could not find a constructor for: '{0}'", targetType ) );
				}
			}

			var result = new Microsoft.Practices.Unity.InjectionConstructor( items.ToArray() );
			return result;
		}

		bool Find( ConstructorInfo constructorInfo, Microsoft.Practices.Unity.InjectionParameterValue[] values )
		{
			var index = 0;
			var parameters = constructorInfo.GetParameters();
			var result = values.Count() < parameters.Count() && values.All( x => x.MatchesType( parameters[index++].ParameterType ) ) && parameters.Skip( index ).All( x => x.IsOptional || x.IsDecoratedWith<OptionalDependencyAttribute>() );
			return result;
		}
	}
}