using System.Reflection;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupAutoDataContext
	{
		public SetupAutoDataContext( IFixture fixture, MethodInfo method )
		{
			Fixture = fixture;
			Method = method;
		}

		public IFixture Fixture { get; }
		public MethodInfo Method { get; }
	}
}