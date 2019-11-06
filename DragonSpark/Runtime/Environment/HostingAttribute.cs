using System;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class HostingAttribute : Attribute, IHosting
	{
		readonly Assembly _assembly;

		protected HostingAttribute(Assembly assembly) => _assembly = assembly;

		public Assembly Get() => _assembly;
	}
}