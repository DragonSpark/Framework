using System;
using System.Reflection;

namespace Prism.Modularity
{
	[Serializable]
	public class ModuleInfoBuilder : ModuleInfoBuilderBase
	{
		protected override string DetermineRef( TypeInfo type )
		{
			return type.Assembly.CodeBase;
		}
	}
}