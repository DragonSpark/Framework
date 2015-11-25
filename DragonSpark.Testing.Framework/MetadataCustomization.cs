using DragonSpark.Activation;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public class MetadataCustomizationFactory : FactoryBase<MethodInfo, ICustomization[]>
	{
		public static MetadataCustomizationFactory Instance { get; } = new MetadataCustomizationFactory();

		protected override ICustomization[] CreateFrom( MethodInfo parameter )
		{
			var type = parameter.DeclaringType;
			var items = type.Transform( t => t.Assembly.GetCustomAttributes() )
					.Concat( type.Transform( t => t.GetCustomAttributes() ) )
					.Concat( parameter.GetCustomAttributes() )
					.OfType<ICustomization>().Prioritize().ToArray();
			return items;
		}
	}

	public class MetadataCustomization : CompositeCustomization
	{
		public MetadataCustomization( MethodInfo methodUnderTest ) : base( MetadataCustomizationFactory.Instance.Create( methodUnderTest ) )
		{}
	}
}