using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

sealed class AssemblyLocation : ReferenceValueStore<Assembly, Uri>
{
	public static AssemblyLocation Default { get; } = new AssemblyLocation();

	AssemblyLocation() : base(Start.A.Selection<Assembly>()
	                               .By.Calling(x => x.Location)
	                               .Select(Start.An.Extent<Uri>().New)
	                               .Get()
	                               .Get) {}
}