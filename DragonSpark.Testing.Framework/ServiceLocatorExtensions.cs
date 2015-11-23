using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Testing.Framework
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