using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DragonSpark
{
	public interface IAllowsPriority
	{
		[DefaultValue( Priority.Normal )]
		Priority Priority { get; }
	}

	public interface IAssemblyLocator
	{
		IEnumerable<Assembly> GetApplicationAssemblies();

		IEnumerable<Assembly> GetAllAssemblies();
	}
}