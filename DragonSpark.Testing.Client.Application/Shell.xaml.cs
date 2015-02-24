
using System;
using System.Diagnostics;

namespace DragonSpark.Testing.Client.Application
{
	/// <summary>
	/// Interaction logic for Shell.xaml
	/// </summary>
	public partial class Shell
	{
		public Shell()
		{
			Trace.WriteLine( "Shell Instantiated." );
			InitializeComponent();
			Trace.WriteLine( "Shell Initialized." );
		}

		protected override void OnActivated( EventArgs e )
		{
			Trace.WriteLine( "Shell Activated." );

			base.OnActivated( e );
		}
	}

	/*public class Source
	{
		public string Message { get; set; }
	}

	public class Target
	{
		public string Description { get; set; } 
	}*/
}
