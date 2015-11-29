using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace DragonSpark.Windows
{
	public static class Process
	{
		static readonly IList<System.Diagnostics.Process> Source = new List<System.Diagnostics.Process>();
		public static IEnumerable<System.Diagnostics.Process> Processes { get; } = new ReadOnlyCollection<System.Diagnostics.Process>( Source );

		[SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Wrapper around native method to internally track processes created by custom application." )]
		public static System.Diagnostics.Process Create( string file )
		{
			var target = System.Diagnostics.Process.Start( file );
			target.NotNull( x =>
			{
				x.Exited += ResultExited;
				Source.Add( x );
			} );
			return target;
		}

		[SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Convenience method." )]
		public static bool Exit( System.Diagnostics.Process process )
		{
			process.Kill();
			process.WaitForExit();
			return process.HasExited;
		}

		static void ResultExited( object sender, EventArgs e )
		{
			Source.Remove( sender.As<System.Diagnostics.Process>() );
		}
	}
}