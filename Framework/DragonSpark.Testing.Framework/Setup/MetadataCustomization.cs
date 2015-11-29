using System.Reflection;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class MetadataCustomization : CompositeCustomization
	{
		public MetadataCustomization( MethodInfo methodUnderTest ) : base( MetadataCustomizationFactory.Instance.Create( methodUnderTest ) )
		{}
	}
}