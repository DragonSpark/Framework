using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public sealed class ApplicationDomainProxyFactory<T> : ParameterizedSourceBase<IEnumerable<object>, T>
	{
		const BindingFlags BindingFlags = System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
		readonly static string AssemblyPath = new Uri( typeof(T).Assembly.CodeBase ).LocalPath;

		public static ApplicationDomainProxyFactory<T> Default { get; } = new ApplicationDomainProxyFactory<T>();
		ApplicationDomainProxyFactory() : this( Defaults.Domain ) {}

		readonly AppDomain domain;

		public ApplicationDomainProxyFactory( AppDomain domain )
		{
			this.domain = domain;
		}

		public override T Get( IEnumerable<object> parameter ) => 
			(T)domain.CreateInstanceFromAndUnwrap( AssemblyPath, typeof(T).FullName, false, BindingFlags, null, parameter.Fixed(), null, null );
	}
}