using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class TypeDefinitions : ParameterizedScope<TypeInfo, TypeInfo>
	{
		public static IParameterizedSource<TypeInfo, TypeInfo> Default { get; } = new TypeDefinitions();
		TypeDefinitions() : base( Factory.GlobalCache<TypeInfo, TypeInfo>( Create ) ) {}

		static TypeInfo Create( TypeInfo parameter )
		{
			foreach ( var provider in TypeSystem.Configuration.TypeDefinitionProviders.Get() )
			{
				var info = provider.Get( parameter );
				if ( info != null )
				{
					return info;
				}
			}
			return null;
		}
	}
}