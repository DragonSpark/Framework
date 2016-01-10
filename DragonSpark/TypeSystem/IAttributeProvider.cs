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
using PostSharp;
using PostSharp.Extensibility;
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
		sealed class Default : AssociatedValue<object, IAttributeProvider>
		{
			public Default( object instance ) : base( instance, () => AttributeProviderFactory.Instance.Create( instance ) ) {}
		}

		sealed class WithRelated : AssociatedValue<object, IAttributeProvider>
		{
			public WithRelated( object instance ) : base( instance, () => AttributeProviderFactory.WithRelated.Create( instance ) ) { }
		}
		
		public static IAttributeProvider Get( object target ) => new Default( target ).Item;

		public static IAttributeProvider Get( MemberInfo target, bool includeRelated ) => includeRelated ? GetWithRelated( target ) : Get( target );

		public static IAttributeProvider GetWithRelated( object target ) => new WithRelated( target ).Item;
	}

	public class AttributeProviderFactory : FirstFactory<object, IAttributeProvider>
	{
		public static AttributeProviderFactory Instance { get; } = new AttributeProviderFactory( false );

		public static AttributeProviderFactory WithRelated { get; } = new AttributeProviderFactory( true );

		public AttributeProviderFactory( bool includeRelated ) : base( new IFactory<object, IAttributeProvider>[]
		{
			new FactoryWithSpecification<IAttributeProvider>( new OfTypeSpecification<Assembly>(), o => new AssemblyAttributeProvider( (Assembly)o ) ),
			new FactoryWithSpecification<IAttributeProvider>( AlwaysSpecification.Instance, o => Activator.Activate<MemberInfoAttributeProviderFactory>().Create( new Tuple<MemberInfo, bool>( o as MemberInfo ?? o.GetType().GetTypeInfo(), includeRelated ) ) )
		} )
		{}
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

		[Cache]
		public bool Contains( Type attribute ) => defined( attribute );

		[Cache]
		public Attribute[] GetAttributes( Type attributeType ) => defined( attributeType ) ? factory( attributeType ).Fixed() : Default<Attribute>.Items;
	}

}