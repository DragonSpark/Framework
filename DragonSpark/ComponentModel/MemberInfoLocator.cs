using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public class MemberInfoLocator : IMemberInfoLocator
	{
		readonly ITypeDefinitionProvider provider;

		public MemberInfoLocator( ITypeDefinitionProvider provider )
		{
			this.provider = provider;
		}

		public MemberInfo Locate( MemberInfo info )
		{
			var typeInfo = info as TypeInfo;
			var key = typeInfo ?? info.DeclaringType.GetTypeInfo();
			var result = provider.GetDefinition( key ).With( x => typeInfo != null ? x : info.AsTo<PropertyInfo, MemberInfo>( y => x.GetDeclaredProperty( info.Name ) ) );
			return result;
		}
	}
}