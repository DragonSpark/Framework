using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class AssemblyLocation : ReferenceValueStore<Assembly, Uri>
	{
		public static AssemblyLocation Default { get; } = new AssemblyLocation();

		AssemblyLocation() : base(Start.A.Selection<Assembly>()
		                               .By.Calling(x => x.CodeBase)
		                               .Select(I.A<Uri>().New)
		                               .Get) {}
	}
}