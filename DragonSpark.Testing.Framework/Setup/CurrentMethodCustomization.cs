using DragonSpark.Testing.Framework.Extensions;
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
			fixture.Items().Add( this );
			new AssociatedFixture( Method ).Assign( fixture );
		}
	}
}