using System.Diagnostics;
using System.Reflection;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class Testes
	{
		Testes()
		{}

		public static Testes Create() => Default<Testes>.Item;
	}

	public class KnownTypeFactoryTests
	{
		[Theory, ConfiguredMoqAutoData]
		public void Testing( Testes sut )
		{
			Debugger.Break();
		}
	}
}