using DragonSpark.Extensions;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ConventionBasedTypeDefinitionProvider : ITypeDefinitionProvider
	{
		public static ConventionBasedTypeDefinitionProvider Instance { get; } = new ConventionBasedTypeDefinitionProvider();

		class Context
		{
			readonly TypeInfo current;
			readonly Lazy<TypeInfo> metadata;

			public Context( TypeInfo current )
			{
				this.current = current;
				metadata = new Lazy<TypeInfo>( ResolveMetadata );
			}

			public Context CreateFromBaseType()
			{
				var result = current.BaseType.With( x => new Context( x.GetTypeInfo() ) );
				return result;
			}
			
			public TypeInfo Metadata => metadata.Value;

			TypeInfo ResolveMetadata()
			{
				var name = $"{current.FullName}Metadata, {current.Assembly.FullName}";
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
}