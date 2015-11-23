using System.Reflection;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
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
			fixture.GetItems().Add( this );
			AmbientValues.RegisterFor( fixture, Method );
		}
	}
}