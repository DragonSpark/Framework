using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using DragonSpark.Runtime;

namespace DragonSpark.Extensions
{
	class MetadataEnabledMemberInfoAttributeProvider : ICustomAttributeProvider
	{
		readonly static Dictionary<Type,Type> MetadataCache = new Dictionary<Type, Type>();
		readonly MemberInfo info;

		public MetadataEnabledMemberInfoAttributeProvider( MemberInfo info )
		{
			this.info = info;
		}

		MemberInfo MetadataInfo
		{
			get { return metadataInfo ?? ( metadataInfo = ResolveMetadataInfo() ); }
		}	MemberInfo metadataInfo;

		MemberInfo ResolveMetadataInfo()
		{
			var metadata = MetadataCache.Ensure( info.ReflectedType, item => FromDecoration( item ) ?? FromConvention( item ) );
			var result = metadata.Transform( item => item.GetMember( info.Name, DragonSparkBindingOptions.AllProperties ).FirstOrDefault() );
			return result;
		}

		class Context
		{
			readonly PropertyContext<Type> metadata;

			public Context( Type current )
			{
				Current = current;
				metadata = new PropertyContext<Type>( ResolveMetadata );
			}

			public Context CreateFromBaseType()
			{
				var result = Current.GetProperty( "Base" ).Transform( item => item.PropertyType, () => Current.BaseType ).Transform( item => new Context( item ) );
				return result;
			}

			Type Current { get; set; }

			public Type Metadata
			{
				get { return metadata.Value; }
			}	

			Type ResolveMetadata()
			{
				var name = string.Format( "{0}Metadata, {1}", Current.FullName, Current.Assembly.FullName );
				var result = Type.GetType( name, false, true );
				return result;
			}
		}

		static Type FromConvention( Type type )
		{
			var context = new Context( type );
			var result = context.ResolveFromParent( 
				item => item.CreateFromBaseType(), 
				item => item.Metadata != null,
				item => item.Metadata
				);
			return result;
		}

		static Type FromDecoration( ICustomAttributeProvider type )
		{
			var result = type.FromMetadata<MetadataTypeAttribute, Type>( item => item.MetadataClassType );
			return result;
		}

		#region Implementation of ICustomAttributeProvider
		public object[] GetCustomAttributes( Type attributeType, bool inherit )
		{
			var attributes = info.GetCustomAttributes( attributeType, inherit );
			var items = MetadataInfo.Transform( item => item.GetCustomAttributes( attributeType, inherit ).Concat( attributes ), () => attributes ) ?? new object[0];
			var result = items.ToArray();
			return result;
		}

		public object[] GetCustomAttributes( bool inherit )
		{
			var attributes = info.GetCustomAttributes( inherit );
			var items = MetadataInfo.Transform( item => item.GetCustomAttributes( inherit ).Concat( attributes ), () => attributes ) ?? new object[0];
			var result = items.ToArray();
			return result;
		}

		public bool IsDefined( Type attributeType, bool inherit )
		{
			var result = GetCustomAttributes( attributeType, inherit ).Any();
			return result;
		}
		#endregion
	}
}