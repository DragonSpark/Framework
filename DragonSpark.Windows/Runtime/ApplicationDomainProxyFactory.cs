using System;
using System.Reflection;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Windows.Runtime
{
	public class ApplicationDomainProxyFactory<T> : ParameterizedSourceBase<object[], T>
	{
		readonly AppDomain domain;

		public ApplicationDomainProxyFactory( AppDomain domain )
		{
			this.domain = domain;
		}

		public override T Get( object[] parameter )
		{
			var assemblyPath = new Uri( typeof(T).Assembly.CodeBase).LocalPath;
			var result = (T)domain.CreateInstanceFromAndUnwrap(assemblyPath, typeof(T).FullName, false
															   , BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Default
															   , null, parameter, null, null );
			return result;
		}
	}
}