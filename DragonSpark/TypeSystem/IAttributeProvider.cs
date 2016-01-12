using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Runtime.Values;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Activation;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.TypeSystem
{
	public class MemberInfoAttributeProviderFactory : FactoryBase<Tuple<MemberInfo, bool>, IAttributeProvider>
	{
		readonly IMemberInfoLocator locator;

		public MemberInfoAttributeProviderFactory() : this( MemberInfoLocator.Instance ) {}

		public MemberInfoAttributeProviderFactory( [Required]IMemberInfoLocator locator )
		{
			this.locator = locator;
		}

		protected override IAttributeProvider CreateItem( Tuple<MemberInfo, bool> parameter ) => new MemberInfoAttributeProvider( locator.Create( parameter.Item1 ) ?? parameter.Item1, parameter.Item2 );
	}

	public static class Attributes
	{
		sealed class Default : AssociatedValue<IAttributeProvider>
		{
			public Default( object instance ) : base( instance, () => new AttributeProviderFactory( false ).Create( instance ) ) {}
		}

		sealed class WithRelated : AssociatedValue<IAttributeProvider>
		{
			public WithRelated( object instance ) : base( instance, () => new AttributeProviderFactory( true ).Create( instance ) ) {}
		}
		
		public static IAttributeProvider Get( object target ) => new Default( target ).Item;

		public static IAttributeProvider Get( MemberInfo target, bool includeRelated ) => includeRelated ? GetWithRelated( target ) : Get( target );

		public static IAttributeProvider GetWithRelated( object target ) => new WithRelated( target ).Item;
	}

	public class AttributeProviderFactory : FirstFromParameterFactory<object, IAttributeProvider>
	{
		public AttributeProviderFactory( bool includeRelated ) : base( IsAssemblyFactory.Instance.Create, new Providerfactory( includeRelated ).Create ) {}

		class Providerfactory : FactoryBase<object, IAttributeProvider>
		{
			readonly MemberInfoAttributeProviderFactory inner;
			readonly bool includeRelated;

			public Providerfactory( bool includeRelated ) : this( Services.Locate<MemberInfoAttributeProviderFactory>(), includeRelated ) {}

			Providerfactory( MemberInfoAttributeProviderFactory inner, bool includeRelated )
			{
				this.inner = inner;
				this.includeRelated = includeRelated;
			}

			protected override IAttributeProvider CreateItem( object parameter )
			{
				var item = new Tuple<MemberInfo, bool>( parameter as MemberInfo ?? parameter.GetType().GetTypeInfo(), includeRelated );
				var result = inner.Create( item );
				return result;
			}
		}

		class IsAssemblyFactory : FactoryWithSpecification<object, IAttributeProvider>
		{
			public static IsAssemblyFactory Instance { get; } = new IsAssemblyFactory();

			public IsAssemblyFactory() : base( IsTypeSpecification<Assembly>.Instance, o => new AssemblyAttributeProvider( (Assembly)o ) ) {}
		}
	}

	public interface IAttributeProvider
	{
		bool Contains( Type attribute );

		Attribute[] GetAttributes( [Required]Type attributeType );
	}

	public class MemberInfoAttributeProvider : AttributeProviderBase
	{
		public MemberInfoAttributeProvider( [Required]MemberInfo info, bool inherit ) : base( type => info.IsDefined( type, inherit ), type => info.GetCustomAttributes( type, inherit ) ) {}
	}

	public class AssemblyAttributeProvider : AttributeProviderBase
	{
		public AssemblyAttributeProvider( [Required]Assembly assembly ) : base( assembly.IsDefined, assembly.GetCustomAttributes ) {}
	}

	public abstract class AttributeProviderBase : IAttributeProvider
	{
		readonly Func<Type, bool> defined;
		readonly Func<Type, IEnumerable<Attribute>> factory;

		protected AttributeProviderBase( [Required]Func<Type, bool> defined, [Required]Func<Type, IEnumerable<Attribute>> factory )
		{
			this.defined = defined;
			this.factory = factory;
		}

		[Freeze]
		public bool Contains( Type attribute ) => defined( attribute );

		[Freeze]
		public Attribute[] GetAttributes( Type attributeType ) => defined( attributeType ) ? factory( attributeType ).Fixed() : Default<Attribute>.Items;
	}

}