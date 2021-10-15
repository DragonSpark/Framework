using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.IO;
using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

sealed class Load : Select<AssemblyName, Assembly>
{
	public static Load Default { get; } = new Load();

	Load() : base(Start.A.Selection<AssemblyName>()
	                   .By.Calling(Assembly.Load)
	                   .Try<FileNotFoundException>()
	                   .Get()) {}
}