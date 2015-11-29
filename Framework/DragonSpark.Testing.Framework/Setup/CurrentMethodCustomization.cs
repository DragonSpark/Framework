using System.Reflection;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Extensions;
using Ploeh.AutoFixture;

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
			AmbientValues.RegisterFor( fixture, Method );
		}
	}
}