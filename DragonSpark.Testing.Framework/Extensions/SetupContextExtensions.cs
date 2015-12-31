using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Setup;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Extensions
{
	public static class SetupContextExtensions
	{
		public static MethodInfo Method( this ISetupParameter @this )
		{
			var result = @this.GetArguments<SetupAutoDataParameter>().Method;
			return result;
		}
	}
}