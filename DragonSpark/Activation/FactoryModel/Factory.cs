using AutoMapper.Internal;
using DragonSpark.Activation.IoC;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;
using Type = System.Type;

namespace DragonSpark.Activation.FactoryModel
{
	public static class Factory
	{
		readonly static TypeAdapter[]
			Types = new[] { typeof(IFactory<>), typeof(IFactory<,>) }.Select( type => type.Adapt() ).ToArray(),
			BasicTypes = new[] { typeof(IFactory), typeof(IFactoryWithParameter) }.Select( type => type.Adapt() ).ToArray();
		
		public static bool IsFactory( [Required] Type type ) => BasicTypes.Any( adapter => adapter.IsAssignableFrom( type ) );

		public static T Create<T>() => (T)From( FrameworkFactoryTypeLocator.Instance.Create( typeof(T) ) );

		public static object From( [Required, OfFactoryType]Type factoryType ) => FactoryDelegateLocatorFactory.Instance.Create( factoryType )();

		[Freeze]
		public static Type GetParameterType( [Required]Type factoryType )
		{
			var result = Get( factoryType, types => types.First(), Types.Last() );
			return result;
		}

		[Freeze]
		public static Type GetInterface( [Required] Type factoryType )
		{
			var result = factoryType.Adapt().GetAllInterfaces().FirstOrDefault( IsFactory );
			return result;
		}

		[Freeze]
		public static Type GetResultType( [Required]Type factoryType ) => Get( factoryType, types => types.Last(), Types );

		static Type Get( Type factoryType, Func<Type[], Type> selector, params TypeAdapter[] typesToCheck )
		{
			var result = factoryType.Append( factoryType.Adapt().GetAllInterfaces() )
				.AsTypeInfos()
				.Where( type => type.IsGenericType && typesToCheck.Any( extension => extension.IsAssignableFrom( type.GetGenericTypeDefinition() ) ) )
				.Select( type => selector( type.GenericTypeArguments ) )
				.FirstOrDefault();
			return result;
		}
	}

	public class FactoryDelegateLocatorFactory : FirstFromParameterFactory<Type, Func<object>>
	{
		public static FactoryDelegateLocatorFactory Instance { get; } = new FactoryDelegateLocatorFactory();

		public FactoryDelegateLocatorFactory() : base( 
			new Factory<IFactoryWithParameter>( FactoryWithParameterContainedDelegateFactory.Instance ),
			new Factory<IFactory>( FactoryDelegateFactory.Instance )
		) {}

		class Factory<T> : FactoryWithSpecification<Type, Func<object>>
		{
			public Factory( IFactory<Type, Func<object>> inner ) : base( TypeAssignableSpecification<T>.Instance, inner.Create ) {}
		}
	}

	public class MemberInfoFactoryTypeLocator : FactoryTypeLocatorBase<MemberInfo>
	{
		public static MemberInfoFactoryTypeLocator Instance { get; } = new MemberInfoFactoryTypeLocator();

		public MemberInfoFactoryTypeLocator() : base( member => member.GetMemberType(), member => new[] { member.DeclaringType } ) {}
	}

	public class ParameterInfoFactoryTypeLocator : FactoryTypeLocatorBase<ParameterInfo>
	{
		public static ParameterInfoFactoryTypeLocator Instance { get; } = new ParameterInfoFactoryTypeLocator();

		public ParameterInfoFactoryTypeLocator() : base( parameter => parameter.ParameterType, parameter => new[] { parameter.Member.DeclaringType } ) {}
	}

	public class FrameworkFactoryTypeLocator : FactoryTypeLocatorBase<Type>
	{
		public static FrameworkFactoryTypeLocator Instance { get; } = new FrameworkFactoryTypeLocator();

		public FrameworkFactoryTypeLocator() : base( Default<Type>.Self, t => Default<Type>.Items ) {}
	}

	public abstract class FactoryTypeLocatorBase<T> : FactoryBase<T, Type>
	{
		readonly Func<T, Type> type;
		readonly Func<T, Type[]> locations;

		protected FactoryTypeLocatorBase( [Required]Func<T, Type> type, [Required]Func<T, Type[]> locations )
		{
			this.type = type;
			this.locations = locations;
		}

		[Freeze]
		protected override Type CreateItem( T parameter )
		{
			var mapped = type( parameter );
			var assemblies = new Assemblies.Get[] { Assemblies.GetCurrent, mapped.Append( locations( parameter ) ).Append( GetType() ).Distinct().Assemblies };

			var result = assemblies.FirstWhere( get => new FactoryTypeLocator( get() ).Create( mapped ) );
			return result;
		}
	}

	public class FactoryTypeLocator : FactoryBase<Type, Type>
	{
		readonly Assembly[] assemblies;

		public FactoryTypeLocator( [Required]Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		protected override Type CreateItem( Type parameter )
		{
			var name = $"{parameter.Name}Factory";
			var result =
				assemblies.FirstWhere( assembly =>
				{
					var buildable = assembly.DefinedTypes.AsTypes().Where( Factory.IsFactory ).Where( CanBuildSpecification.Instance.IsSatisfiedBy ).ToArray();
					return buildable.With( types =>
						types.Where( info => info.Name == name ).Only()
						??
						types.Select( type => new { type, resultType = Factory.GetResultType( type ) } ).NotNull( arg => arg.resultType ).ToArray().With( pairs =>
						{
							return pairs.Where( arg => arg.resultType == parameter ).Concat( pairs.Where( arg => parameter.Adapt().IsAssignableFrom( arg.resultType ) ) ).WithFirst( arg => arg.type );
						} )
					);
				} );
			return result;
		}
	}
}