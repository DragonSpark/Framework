using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Modularity
{
	public class AttributeDataProvider : IAttributeDataProvider
	{
		public static AttributeDataProvider Instance { get; } = new AttributeDataProvider();

		public T Get<T>( Type attributeType, Type type, string name )
		{
			var attribute = Get( attributeType, type );
			var result = attribute != null ? GetDeclaredProperty<T>( attribute, attributeType, name ) : default(T);
			return result;
		}

		static Attribute Get( Type attributeType, Type type )
		{
			return type.GetTypeInfo().GetCustomAttribute( attributeType );
		}

		static T GetDeclaredProperty<T>( Attribute attribute, Type attributeType, string name )
		{
			var info = attributeType.GetTypeInfo().GetDeclaredProperty( name );
			var result = info != null ? (T)info.GetValue( attribute ) : default(T);
			return result;
		}

		public IEnumerable<T> GetAll<T>( Type attributeType, Type type, string name )
		{
			var attributes = type.GetTypeInfo().GetCustomAttributes( attributeType );
			var result = attributes.Select( attribute => GetDeclaredProperty<T>( attribute, attributeType, name ) ).NotNull().ToArray();
			return result;
		}
	}
}