
using System;
using System.Diagnostics;
using System.Windows;
using DragonSpark.Application.Client.Forms;
using DragonSpark.Extensions;

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

		void ButtonBase_OnClick( object sender, RoutedEventArgs e )
		{
			Trigger.Source.As<SystemAlert>( alert => alert.Test() );
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
