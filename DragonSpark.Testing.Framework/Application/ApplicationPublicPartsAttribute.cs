using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationPublicPartsAttribute : TypeProviderAttributeBase
	{
		public ApplicationPublicPartsAttribute() : base( m => PublicParts.Default.Get( m.DeclaringType.Assembly ) ) {}
	}
}