using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;
using PostSharp.Extensibility;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Aspects
{
	[AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	[AttributeUsage( AttributeTargets.Method ), MulticastAttributeUsage( MulticastTargets.Method | MulticastTargets.InstanceConstructor, TargetMemberAttributes = MulticastAttributes.NonAbstract ), LinesOfCodeAvoided( 1 )]
	public sealed class AutoGuardAspect : MethodLevelAspect, IAspectProvider
	{
		readonly static IParameterizedSource<Type, Type>[] DefaultSources = { DefaultSource.Default, ImmutableArraySource.Default };

		readonly IParameterizedSource<Type, Type>[] sources;

		public AutoGuardAspect() : this( DefaultSources ) {}

		public AutoGuardAspect( params IParameterizedSource<Type, Type>[] sources )
		{
			this.sources = sources;
		}

		public override bool CompileTimeValidate( MethodBase method ) => ( !method.IsSpecialName || method is ConstructorInfo ) && method.GetParameters().Any();

		public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
		{
			var methodBase = (MethodBase)targetElement;
			foreach ( var parameter in methodBase.GetParameters().Where( info => !info.IsOptional ) )
			{
				var parameterType = parameter.ParameterType;
				foreach ( var source in sources )
				{
					var type = source.Get( parameterType );
					if ( type != null )
					{
						yield return new AspectInstance( parameter, new ObjectConstruction( type ), null ) { RepresentAsStandalone = true };
						break;
					}
				}
			}
		}

		sealed class DefaultSource : ParameterizedSourceBase<Type, Type>
		{
			public static IParameterizedSource<Type, Type> Default { get; } = new DefaultSource().Apply( Specification.Implementation );
			DefaultSource() {}

			public override Type Get( Type parameter ) => parameter == typeof(string) ? typeof(RequiredAttribute) : typeof(NotNullAttribute);

			sealed class Specification : SpecificationBase<Type>
			{
				public static ISpecification<Type> Implementation { get; } = new DelegatedSpecification<Type>( new DecoratedSourceCache<Type, bool>( new Specification().IsSatisfiedBy ).Get );
				Specification() {}

				public override bool IsSatisfiedBy( Type parameter ) => !parameter.IsByRef && Nullable.GetUnderlyingType( parameter ) == null && !parameter.GetTypeInfo().IsValueType;
			}
		}

		sealed class ImmutableArraySource : ParameterizedSourceBase<Type, Type>
		{
			public static IParameterizedSource<Type, Type> Default { get; } = new ImmutableArraySource().Apply( GenericTypeAssignableSpecification.Defaults.Get( typeof(ImmutableArray<>) ) );
			ImmutableArraySource() {}

			public override Type Get( Type parameter ) => typeof(AssignedAttribute);
		}
	}
}
