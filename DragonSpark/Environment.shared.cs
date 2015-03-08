
using System.Security.Permissions;

namespace DragonSpark
{
    public static partial class Environment
	{
	    /// <summary>
		/// Indicates whether or not the framework is in design-time mode.
		/// </summary>
		public static bool IsInDesignMode
		{
			get { return (bool)( IsInDesignModeField ?? ( IsInDesignModeField = ResolveDesignMode() ) ); }
		}	static bool? IsInDesignModeField;
	}
}