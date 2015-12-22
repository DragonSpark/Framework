using System.Reflection;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Extensions;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CurrentMethodCustomization : ICustomization, ITestExecutionAware
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

		void ITestExecutionAware.Before( IFixture fixture, MethodInfo methodUnderTest )
		{
			new CurrentMethodValue().Assign( methodUnderTest );
		}

		void ITestExecutionAware.After( IFixture fixture, MethodInfo methodUnderTest )
		{
			new CurrentMethodValue().Assign( null );
		}
	}
}