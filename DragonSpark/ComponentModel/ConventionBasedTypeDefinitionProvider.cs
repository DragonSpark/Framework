using System;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	/*public interface IAttributeProvider
	{
		Attribute[] GetCustomAttributes( bool inherit );
	
		Attribute[] GetCustomAttributes( Type attributeType, bool inherit );
		
		bool IsDefined( Type attributeType, bool inherit );
	}*/

	/*public interface IAttributeProviderFactory : IFactory
	{}

	class AttributeProviderFactory : Factory<Type, IAttributeProvider>, IAttributeProviderFactory
	{
		protected override IAttributeProvider CreateItem( Type parameter )
		{
			return new AttributeProvider( parameter.GetTypeInfo() );
		}
	}*/

	/*public interface ISpecification
	{
		bool IsSatisfiedBy( object payload );
	}*/

	/*public interface IAttributeMetadataProvider
	{
		TypeInfo GetMetadata( TypeInfo info );
	}*/

	public class ConventionBasedTypeDefinitionProvider : ITypeDefinitionProvider
	{
		class Context
		{
			readonly Lazy<TypeInfo> metadata;

			public Context( TypeInfo current )
			{
				Current = current;
				metadata = new Lazy<TypeInfo>( ResolveMetadata );
			}

			public Context CreateFromBaseType()
			{
				var result =  Current.BaseType.With( x => new Context( IntrospectionExtensions.GetTypeInfo( x ) ) );
				return result;
			}

			TypeInfo Current { get; set; }

			public TypeInfo Metadata
			{
				get { return metadata.Value; }
			}

			TypeInfo ResolveMetadata()
			{
				var name = string.Format( "{0}Metadata, {1}", Current.FullName, Current.Assembly.FullName );
				var result = Type.GetType( name, false ).With( x => x.GetTypeInfo() );
				return result;
			}
		}

		public TypeInfo GetDefinition( TypeInfo info )
		{
			var context = new Context( info );
			var result = context.Loop( 
				item => item.CreateFromBaseType(), 
				item => item.Metadata != null,
				item => item.Metadata
				);
			return result;
		}
	}

	/*public class AttributeMetadataProviderRelay : IAttributeMetadataProvider
	{
		readonly ITypeDefinitionProvider provider;

		public AttributeMetadataProviderRelay( ITypeDefinitionProvider provider )
		{
			this.provider = provider;
		}

		public TypeInfo GetMetadata( TypeInfo info )
		{
			var result = provider.GetDefinition( info );
			return result;
		}
	}*/

	/*class AttributeProvider : IAttributeProvider
	{
		readonly MemberInfo info;
		readonly Lazy<MemberInfo> descriptor;

		public AttributeProvider( MemberInfo info )
		{
			this.info = info;

			descriptor = new Lazy<MemberInfo>( () =>
			{
				var result = provider.GetDefinition( key ).Transform( item => item.GetDeclaredProperty( info.Name ) );
				return result;
			} );
		}

		Attribute[] Get( Func<MemberInfo,IEnumerable<Attribute>> getter )
		{
			var attributes = getter( info ).ToArray();
			var items = descriptor.Value.Transform( item => getter( item ).Concat( attributes ), () => attributes );
			var result = items.ToArray();
			return result;
		}

		public Attribute[] GetCustomAttributes( Type attributeType, bool inherit )
		{
			var result = Get( x => x.GetCustomAttributes( attributeType, inherit ) );
			return result;
		}

		public Attribute[] GetCustomAttributes( bool inherit )
		{
			var result = Get( x => x.GetCustomAttributes( inherit ) );
			return result;
		}

		public bool IsDefined( Type attributeType, bool inherit )
		{
			var result = GetCustomAttributes( attributeType, inherit ).Any();
			return result;
		}
	}*/
}