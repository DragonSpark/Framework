using DragonSpark.Activation.FactoryModel;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class MetadataCustomizationFactory : FactoryBase<MethodBase, ICustomization[]>
	{
		public static MetadataCustomizationFactory Instance { get; } = new MetadataCustomizationFactory();

		protected override ICustomization[] CreateItem( MethodBase parameter )
		{
			var customizations = new object[] { parameter, parameter.DeclaringType, parameter.DeclaringType.Assembly }
				.SelectMany( HostedValueLocator<ICustomization>.Instance.Create )
				.Prioritize()
				.Fixed();
			return customizations;
		}
	}
}