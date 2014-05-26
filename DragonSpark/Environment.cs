
namespace DragonSpark
{
	public static class Environment
	{
		/*[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification =  "Used to determine the Environment Design Mode." )]
		static bool ResolveDesignMode()
		{
			var prop = DesignerProperties.IsInDesignModeProperty;
			var result = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue || ( !IsInDesignModeField.GetValueOrDefault(false) && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal) );
			return result;
		}

		/// <summary>
		/// Indicates whether or not the framework is in design-time mode.
		/// </summary>
		public static bool IsInDesignMode
		{
			get { return (bool)( IsInDesignModeField ?? ( IsInDesignModeField = ResolveDesignMode() ) ); }
		}	static bool? IsInDesignModeField;*/
	}
}
