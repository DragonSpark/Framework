using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class MetadataCustomizationFactory : FactoryBase<MethodInfo, ICustomization[]>
	{
		public static MetadataCustomizationFactory Instance { get; } = new MetadataCustomizationFactory();

		protected override ICustomization[] CreateItem( MethodInfo parameter )
		{
			var type = parameter.DeclaringType;
			var items = type.With( t => t.Assembly.GetCustomAttributes() )
				.Concat( type.With( t => t.GetCustomAttributes() ) )
				.Concat( parameter.GetCustomAttributes() )
				.OfType<ICustomization>().Prioritize().ToArray();
			return items;
		}
	}
}