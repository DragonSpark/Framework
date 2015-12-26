using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Extensions
{
	public static class SetupContextExtensions
	{
		public static IFixture Fixture( this ISetupParameter @this )
		{
			var result = @this.GetArguments<SetupAutoDataParameter>().Fixture;
			return result;
		}

		public static MethodInfo Method( this ISetupParameter @this )
		{
			var result = @this.GetArguments<SetupAutoDataParameter>().Method;
			return result;
		}
	}
}