using System.Reflection;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public interface ITestExecutionAware
	{
		void Before( IFixture fixture, MethodInfo methodUnderTest );

		void After( IFixture fixture, MethodInfo methodUnderTest );
	}
}