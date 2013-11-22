using System.Reflection;

namespace DragonSpark.Server.ClientHosting
{
	public class AssemblyResource
	{
		readonly Assembly assembly;
		readonly string resourceName;

		public AssemblyResource( Assembly assembly, string resourceName )
		{
			this.assembly = assembly;
			this.resourceName = resourceName;
		}

		public Assembly Assembly
		{
			get { return assembly; }
		}

		public string ResourceName
		{
			get { return resourceName; }
		}
	}
}