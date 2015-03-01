using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Activation
{
	public interface IAssembliesProvider
	{
		IList<Assembly> Assemblies { get; }
	}
}