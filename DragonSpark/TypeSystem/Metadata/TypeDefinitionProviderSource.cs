using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public class TypeDefinitionProviderSource : ItemSource<ITypeDefinitionProvider>
	{
		public static TypeDefinitionProviderSource Default { get; } = new TypeDefinitionProviderSource();
		TypeDefinitionProviderSource() : this( Items<ITypeDefinitionProvider>.Default ) {}

		protected TypeDefinitionProviderSource( params ITypeDefinitionProvider[] items ) : base( items.Append( ConventionTypeDefinitionProvider.Default, Self.DefaultNested ) ) {}

		sealed class Self : SelfAlteration<TypeInfo>, ITypeDefinitionProvider
		{
			public static Self DefaultNested { get; } = new Self();
			Self() {}
		}
	}
}