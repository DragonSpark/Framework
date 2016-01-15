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
		public static MemberInfoAttributeProviderFactory Instance { get; } = new MemberInfoAttributeProviderFactory();

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
		sealed class Cached<T> : AssociatedValue<IAttributeProvider> where T : AttributeProviderFactoryBase
		{
			public Cached( object instance ) : base( instance, () =>
			{
				var activator = Services.Locate<T>();
				var result = activator.Create( instance );
				return result;
			} ) {}
		}

		public static IAttributeProvider Get( object target ) => new Cached<AttributeProviderFactory>( target ).Item;

		public static IAttributeProvider Get( MemberInfo target, bool includeRelated ) => includeRelated ? GetWithRelated( target ) : Get( target );

		public static IAttributeProvider GetWithRelated( object target ) => new Cached<ExpandedAttributeProviderFactory>( target ).Item;
	}

	class MemberInfoProviderFactory : MemberInfoProviderFactoryBase
	{
		public static MemberInfoProviderFactory Instance { get; } = new MemberInfoProviderFactory();

		public MemberInfoProviderFactory() : this( new MemberInfoAttributeProviderFactory( new MemberInfoLocator() ) ) {}

		public MemberInfoProviderFactory( MemberInfoAttributeProviderFactory inner ) : base( inner, false ) {}
	}

	abstract class MemberInfoProviderFactoryBase : FactoryBase<object, IAttributeProvider>
	{
		readonly MemberInfoAttributeProviderFactory inner;
		readonly bool includeRelated;

		protected MemberInfoProviderFactoryBase( [Required]MemberInfoAttributeProviderFactory inner, bool includeRelated )
		{
			this.inner = inner;
			this.includeRelated = includeRelated;
		}

		protected override IAttributeProvider CreateItem( object parameter )
		{
			var item = new Tuple<MemberInfo, bool>( parameter as MemberInfo ?? ( parameter as System.Type ?? parameter.GetType() ).GetTypeInfo(), includeRelated );
			var result = inner.Create( item );
			return result;
		}
	}

	class ExpandedAttributeProviderFactory : AttributeProviderFactoryBase
	{
		public ExpandedAttributeProviderFactory() : this( MemberInfoWithRelatedProviderFactory.Instance ) {}

		public ExpandedAttributeProviderFactory( MemberInfoWithRelatedProviderFactory factory ) : base( factory ) {}
	}

	class AttributeProviderFactory : AttributeProviderFactoryBase
	{
		public AttributeProviderFactory() : this( MemberInfoProviderFactory.Instance ) {}

		public AttributeProviderFactory( MemberInfoProviderFactory factory ) : base( factory ) {}
	}

	abstract class AttributeProviderFactoryBase : FirstFromParameterFactory<object, IAttributeProvider>
	{
		protected AttributeProviderFactoryBase( MemberInfoProviderFactoryBase factory ) : base( IsAssemblyFactory.Instance.Create, factory.Create ) {}

		class IsAssemblyFactory : FactoryWithSpecification<object, IAttributeProvider>
		{
			public static IsAssemblyFactory Instance { get; } = new IsAssemblyFactory();

			IsAssemblyFactory() : base( IsTypeSpecification<Assembly>.Instance, o => new AssemblyAttributeProvider( (Assembly)o ) ) {}
		}
	}

	public interface IAttributeProvider
	{
		bool Contains( System.Type attribute );

		Attribute[] GetAttributes( [Required]System.Type attributeType );
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
		readonly Func<System.Type, bool> defined;
		readonly Func<System.Type, IEnumerable<Attribute>> factory;

		protected AttributeProviderBase( [Required]Func<System.Type, bool> defined, [Required]Func<System.Type, IEnumerable<Attribute>> factory )
		{
			this.defined = defined;
			this.factory = factory;
		}

		[Freeze]
		public bool Contains( System.Type attribute ) => defined( attribute );

		[Freeze]
		public Attribute[] GetAttributes( System.Type attributeType ) => defined( attributeType ) ? factory( attributeType ).Fixed() : Default<Attribute>.Items;
	}

}