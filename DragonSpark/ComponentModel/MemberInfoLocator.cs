using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class MemberInfoLocator : TransformerBase<MemberInfo>, IMemberInfoLocator
	{
		public static MemberInfoLocator Instance { get; } = new MemberInfoLocator();

		readonly ITypeDefinitionProvider provider;

		public MemberInfoLocator() : this( TypeDefinitionProvider.Instance )
		{}

		public MemberInfoLocator( ITypeDefinitionProvider provider )
		{
			this.provider = provider;
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

		protected override MemberInfo CreateItem( MemberInfo parameter )
		{
			var typeInfo = parameter as TypeInfo;
			var key = typeInfo ?? parameter.DeclaringType.GetTypeInfo();
			var result = provider.GetDefinition( key ).With( x => typeInfo != null ? x : parameter.AsTo<PropertyInfo, MemberInfo>( y => GetDeclaredProperty( parameter, x ) ) );
			return result;
		}
	}
}