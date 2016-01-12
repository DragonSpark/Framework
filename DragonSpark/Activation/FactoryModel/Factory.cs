using AutoMapper.Internal;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.FactoryModel
{
	public static class Factory
	{
		readonly internal static TypeAdapter[]
			Types = new[] { typeof(IFactory<>), typeof(IFactory<,>) }.Select( type => type.Adapt() ).ToArray();

		readonly internal static TypeAdapter[]
			BasicTypes = new[] { typeof(IFactory), typeof(IFactoryWithParameter) }.Select( type => type.Adapt() ).ToArray();


		public static T Create<T>() => (T)From( FactoryTypeLocator.Instance.Create( typeof(T) ) );

		public static object From( [OfFactoryType]Type factoryType ) => FactoryDelegateLocatorFactory.Instance.Create( factoryType )();

		[Freeze]
		public static Type GetParameterType( [Required]Type factoryType )
		{
			var result = Get( factoryType, types => types.First(), Types.Last() );
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

		public MemberInfoFactoryTypeLocator() : base( member => new[] { member.GetMemberType(), member.DeclaringType } ) {}
	}

	public class ParameterInfoFactoryTypeLocator : FactoryTypeLocatorBase<ParameterInfo>
	{
		public static ParameterInfoFactoryTypeLocator Instance { get; } = new ParameterInfoFactoryTypeLocator();

		public ParameterInfoFactoryTypeLocator() : base( parameter => new[] { parameter.ParameterType, parameter.Member.DeclaringType } ) {}
	}

	public abstract class FactoryTypeLocatorBase<T> : FactoryBase<T, Type>
	{
		readonly Func<T, Type[]> types;

		protected FactoryTypeLocatorBase( [Required]Func<T, Type[]> types )
		{
			this.types = types;
		}

		[Freeze]
		protected override Type CreateItem( T parameter )
		{
			var candidates = types( parameter );
			var result = candidates.FirstWhere( FactoryTypeLocator.Instance.Create );
			return result;
		}
	}

	public class FactoryTypeLocator : FactoryBase<Type, Type>
	{
		public static FactoryTypeLocator Instance { get; } = new FactoryTypeLocator();

		readonly Assemblies.Get assemblies;

		public FactoryTypeLocator() : this( Assemblies.GetCurrent ) {}

		public FactoryTypeLocator( [Required]Assemblies.Get assemblies )
		{
			this.assemblies = assemblies;
		}

		[Freeze]
		protected override Type CreateItem( Type parameter )
		{
			var name = $"{parameter.Name}Factory";
			var result =
				parameter.GetTypeInfo().DeclaredNestedTypes.AsTypes().Where( info => info.Name == name ).Only()
				??
				assemblies.Or( parameter.Assembly ).FirstWhere( assembly =>
				{
					return assembly.DefinedTypes.AsTypes().Where( x => Factory.BasicTypes.Any( extension => extension.IsAssignableFrom( x ) ) ).ToArray().With( types =>
						types.Where( info => info.Name == name ).Only()
						??
						types.Select( type => new { type, resultType = Factory.GetResultType( type ) } ).ToArray().With( pairs =>
						{
							return pairs.Where( arg => arg.resultType == parameter ).Concat( pairs.Where( arg => parameter.Adapt().IsAssignableFrom( arg.resultType ) ) ).WithFirst( arg => arg.type );
						} )
					);
				} );
			return result;
		}

		// Assembly[] Assemblies => assemblies() ?? Default<Assembly>.Items;
	}
}