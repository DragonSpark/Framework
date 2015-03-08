using System.ComponentModel;

namespace DragonSpark
{
	partial class Environment
	{
		static bool ResolveDesignMode()
		{
			return DesignerProperties.IsInDesignTool;
		}
	}
}
