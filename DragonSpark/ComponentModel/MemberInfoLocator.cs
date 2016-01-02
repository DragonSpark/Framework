using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public class MemberInfoLocator : IMemberInfoLocator
	{
		public static MemberInfoLocator Instance { get; } = new MemberInfoLocator();

		readonly ITypeDefinitionProvider provider;

		public MemberInfoLocator() : this( ConventionBasedTypeDefinitionProvider.Instance )
		{}

		public MemberInfoLocator( ITypeDefinitionProvider provider )
		{
			this.provider = provider;
		}

		public MemberInfo Locate( MemberInfo info )
		{
			var typeInfo = info as TypeInfo;
			var key = typeInfo ?? info.DeclaringType.GetTypeInfo();
			var result = provider.GetDefinition( key ).With( x => typeInfo != null ? x : info.AsTo<PropertyInfo, MemberInfo>( y => GetDeclaredProperty( info, x ) ) );
			return result;
		}

		static PropertyInfo GetDeclaredProperty( MemberInfo info, TypeInfo x )
		{
			try
			{
				return x.GetDeclaredProperty( info.Name );
			}
			catch ( AmbiguousMatchException )
			{
				var result = x.DeclaredProperties.FirstOrDefault( propertyInfo => propertyInfo.Name == info.Name );
				return result;
			}
		}
	}
}