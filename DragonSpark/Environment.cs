using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace DragonSpark
{
    static partial class Environment
	{
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification =  "Used to determine the Environment Design Mode." )]
        static bool ResolveDesignMode()
		{
			var prop = DesignerProperties.IsInDesignModeProperty;
			var result = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue || ( !IsInDesignModeField.GetValueOrDefault(false) && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal) );
			return result;
		}
	}
}
