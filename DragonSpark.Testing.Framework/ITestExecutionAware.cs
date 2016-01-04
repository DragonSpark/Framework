using System.Reflection;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public interface ITestExecutionAware
	{
		void Before( SetupAutoData context );

		void After( SetupAutoData context );
	}
}