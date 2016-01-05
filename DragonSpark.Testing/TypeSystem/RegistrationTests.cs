using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class RegistrationTests
	{
		[RegisterFactory( typeof(AssemblyProvider) )]
		[Theory, AutoDataRegistration]
		public void Testing( Assembly[] sut )
		{
			Assert.Same( sut, AssemblyProvider.Result );
		}

		public class AssemblyProvider : AssemblyProviderBase
		{
			readonly internal static Assembly[] Result = new Assembly[0];

			protected override Assembly[] CreateItem() => Result;
		}
	}
}