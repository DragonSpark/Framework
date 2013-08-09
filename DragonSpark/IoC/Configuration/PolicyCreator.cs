using System.Windows.Markup;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Instance" )]
	public class PolicyCreator : PolicyCreatorBase
	{
		public new IBuilderPolicy Instance { get; set; }

		protected override IBuilderPolicy CreateInstance()
		{
			return Instance;
		}
	}
}