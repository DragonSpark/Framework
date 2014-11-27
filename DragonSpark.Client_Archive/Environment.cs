using System;

namespace DragonSpark
{
	public static class Environment
	{
		static bool ResolveDesignMode()
		{
			var prop = DesignerProperties.IsInDesignModeProperty;
			var result = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue || ( !IsInDesignModeField.GetValueOrDefault(false) && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal) );
			return result;
		}

		public static bool IsInDesignMode
		{
			get { return (bool)( IsInDesignModeField ?? ( IsInDesignModeField = ResolveDesignMode() ) ); }
		}	static bool? IsInDesignModeField;
	}
}
