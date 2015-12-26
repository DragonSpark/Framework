using Ploeh.AutoFixture;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CurrentMethodCustomization : ICustomization
	{
		public CurrentMethodCustomization( MethodInfo method )
		{
			Method = method;
		}

		public MethodInfo Method { get; }

		public void Customize( IFixture fixture )
		{
			new AssociatedFixture( Method ).Assign( fixture );
		}
	}
}