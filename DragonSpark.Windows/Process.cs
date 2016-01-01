using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Windows
{
	public static class Process
	{
		readonly static IList<System.Diagnostics.Process> Source = new List<System.Diagnostics.Process>();
		public static IEnumerable<System.Diagnostics.Process> Processes { get; } = new ReadOnlyCollection<System.Diagnostics.Process>( Source );

		public static System.Diagnostics.Process Create( string file )
		{
			var target = System.Diagnostics.Process.Start( file );
			target.With( x =>
			{
				x.Exited += ResultExited;
				Source.Add( x );
			} );
			return target;
		}

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