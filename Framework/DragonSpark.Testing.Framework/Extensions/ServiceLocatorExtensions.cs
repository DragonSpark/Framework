using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Testing.Framework.Extensions
{
	public static class ServiceLocatorExtensions
	{
		public static IServiceLocator Prepared( this IServiceLocator @this, MethodBase method )
		{
			var key = method.AsTo<MethodInfo, IAmbientKey>( AmbientLocatorKeyFactory.Instance.Create );
			@this.Register( key );
			return @this;
		}
	}
}