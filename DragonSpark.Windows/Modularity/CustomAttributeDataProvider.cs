using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Modularity;

namespace DragonSpark.Windows.Modularity
{
	[Serializable]
	public class CustomAttributeDataProvider : IAttributeDataProvider
	{
		public static CustomAttributeDataProvider Instance { get; } = new CustomAttributeDataProvider();

		public T Get<T>( Type attributeType, Type type, string name )
		{
			var item = Candidates<T>( attributeType, type ).FirstOrDefault();

			var result = item != null ? Get<T>( item, name ) : default(T);
			return result;
		}

		static T Get<T>( CustomAttributeData data, string name )
		{
			var argument = data.NamedArguments.SingleOrDefault( a => a.MemberInfo.Name == name );
			var result = argument.MemberInfo != null ? (T)argument.TypedValue.Value : Position<T>( data );
			return result;
		}

		static T Position<T>( CustomAttributeData data )
		{
			var argument = data.ConstructorArguments.FirstOrDefault( typedArgument => typeof(T).IsAssignableFrom( typedArgument.ArgumentType ) );
			var result = argument.ArgumentType != null ? (T)argument.Value : Position<T>( data );
			return result;
		}

		public IEnumerable<T> GetAll<T>( Type attributeType, Type type, string name )
		{
			var candidates = Candidates<T>( attributeType, type );

			var result = candidates.Select( cad => Get<T>( cad, name ) ).ToArray();
			return result;
		}

		static IEnumerable<CustomAttributeData> Candidates<T>( Type attributeType, Type type )
		{
			return CustomAttributeData.GetCustomAttributes(type)
				.Where( cad => attributeType.IsAssignableFrom( Type.GetType( cad.AttributeType.AssemblyQualifiedName ) ) );
		}
	}
}