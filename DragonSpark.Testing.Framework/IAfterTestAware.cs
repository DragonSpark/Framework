using System.Reflection;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public interface IAfterTestAware
	{
		void After( IFixture fixture, MethodInfo methodUnderTest );
	}
}