using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Runtime
{
	public static class Process
	{
		static readonly IList<System.Diagnostics.Process> Source = new List<System.Diagnostics.Process>();
		static readonly IEnumerable<System.Diagnostics.Process> ProcessesField = new ReadOnlyCollection<System.Diagnostics.Process>( Source );

		public static IEnumerable<System.Diagnostics.Process> Processes
		{
			get { return ProcessesField; }
		}

		public static System.Diagnostics.Process Create( string file )
		{
			var result = System.Diagnostics.Process.Start( file );
			result.NotNull( x =>
			                	{
			                		x.Exited += ResultExited;
			                		Source.Add( x );
			                	} );
			return result;
		}

        public static IEnumerable<System.Diagnostics.Process> GetProcesses()
        {
            var result = System.Diagnostics.Process.GetProcesses().Where( Validate ).OrderBy( x => x.ProcessName ).ToArray();
            return result;
        }

        static bool Validate( System.Diagnostics.Process item )
        {
            try
            {
                var module = item.MainModule;
                return true;
            }
            catch ( Win32Exception )
            {
                return false;   
            }
        }
            
        public static bool Exit( System.Diagnostics.Process process )
		{
			process.Kill();
			process.WaitForExit();
			var result = process.HasExited;
			return result;
		}

		static void ResultExited(object sender, EventArgs e)
		{
			Source.Remove( sender.As<System.Diagnostics.Process>() );
		}
	}
}