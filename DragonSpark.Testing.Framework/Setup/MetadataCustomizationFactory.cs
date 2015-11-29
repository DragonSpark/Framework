using System.Linq;
using System.Reflection;
using DragonSpark.Activation.Build;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class MetadataCustomizationFactory : FactoryBase<MethodInfo, ICustomization[]>
	{
		public static MetadataCustomizationFactory Instance { get; } = new MetadataCustomizationFactory();

		protected override ICustomization[] CreateItem( MethodInfo parameter )
		{
			var type = parameter.DeclaringType;
			var items = type.Transform( t => CustomAttributeExtensions.GetCustomAttributes( (Assembly)t.Assembly ) )
				.Concat( type.Transform( t => t.GetCustomAttributes() ) )
				.Concat( parameter.GetCustomAttributes() )
				.OfType<ICustomization>().Prioritize().ToArray();
			return items;
		}
	}
}