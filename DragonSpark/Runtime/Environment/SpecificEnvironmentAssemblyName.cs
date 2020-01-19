using DragonSpark.Model.Selection.Alterations;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class SpecificEnvironmentAssemblyName : IAlteration<AssemblyName>
	{
		readonly string _format;
		readonly string _name;

		public SpecificEnvironmentAssemblyName(string name) : this("{0}.Environment.{1}", name) {}

		public SpecificEnvironmentAssemblyName(string format, string name)
		{
			_format = format;
			_name   = name;
		}

		public AssemblyName Get(AssemblyName parameter)
			=> new AssemblyName(string.Format(_format, parameter.Name, _name));
	}
}