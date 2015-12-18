using System.Reflection;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Extensions
{
	public static class SetupContextExtensions
	{
		public static IFixture Fixture( this SetupContext @this )
		{
			var result = @this.Item<SetupAutoDataContext>().Fixture;
			return result;
		}

		public static MethodInfo Method( this SetupContext @this )
		{
			var result = @this.Item<SetupAutoDataContext>().Method;
			return result;
		}
	}
}