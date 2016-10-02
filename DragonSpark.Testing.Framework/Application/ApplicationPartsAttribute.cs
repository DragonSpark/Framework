using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationPartsAttribute : TypeProviderAttributeBase
	{
		public ApplicationPartsAttribute() : base( m => AllParts.Default.Get( m.DeclaringType.Assembly ) ) {}
	}
}