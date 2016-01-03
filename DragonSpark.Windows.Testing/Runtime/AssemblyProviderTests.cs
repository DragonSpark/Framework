using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class AssemblyProviderTests
	{
		[Theory, MoqAutoData]
		public void Assemblies( AssemblyProvider sut )
		{
			Assert.NotEqual( sut, AssemblyProvider.Instance );
			Assert.True( sut.Create().All( assembly => assembly.IsDefined( typeof(RegistrationAttribute), false ) ) );
		} 
	}
}