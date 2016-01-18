using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class RegistrationTests
	{
		[RegisterFactory( typeof(AssemblySource) )]
		[Theory, AutoData]
		public void Testing( Assembly[] sut )
		{
			Assert.Same( sut, AssemblySource.Result );
		}

		[Discoverable]
		public class AssemblySource : AssemblySourceBase
		{
			readonly internal static Assembly[] Result = new Assembly[0];

			protected override Assembly[] CreateItem() => Result;
		}
	}
}