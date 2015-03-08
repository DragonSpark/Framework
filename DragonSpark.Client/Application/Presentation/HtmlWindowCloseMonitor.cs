using System;
using System.Windows.Browser;

namespace DragonSpark.Application.Presentation
{
	public class HtmlWindowCloseMonitor
	{
		/// <summary>Fires when immediately before the window closes.</summary>
		public event EventHandler<HtmlWindowCloseEventArgs> WindowClosing = delegate { };

		const string ScriptableObjectName = "HtmlWindowCloseMonitorBridge";
		const string DefaultDialogMessage = "Are you sure you want to close the application?";

		public static HtmlWindowCloseMonitor Instance
		{
			get { return InstanceField; }
		}	static readonly HtmlWindowCloseMonitor InstanceField = new HtmlWindowCloseMonitor();

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "onbeforeunload")]
		HtmlWindowCloseMonitor()
		{
			// Register the scriptable callback member.
			HtmlPage.RegisterScriptableObject( ScriptableObjectName, this );

			// Retrieve the name of the plugin.
			var pluginName = HtmlPage.Plugin.Id;
			if ( string.IsNullOrEmpty( pluginName ) )
			{
				throw new InvalidOperationException( "Cannot register the 'onbeforeunload' event because the Silverlight <object> does not have an ID. Add an ID attribute to the Silverlight <object> host tag." );
			}

			// Wire up event.
			var eventFunction = string.Format(
				@"window.onbeforeunload = function () {{
									var slApp = document.getElementById('{0}');
									var result = slApp.Content.{1}.OnBeforeUnload();
									if(result != null && result.length > 0)
										return result;
								}}", pluginName, ScriptableObjectName );
			HtmlPage.Window.Eval( eventFunction );
		}

		[ScriptableMember]
		public string OnBeforeUnload()
		{
			var args = new HtmlWindowCloseEventArgs();
			WindowClosing( this, args );
			var result = args.Cancel ? ( args.DialogMessage ?? DefaultDialogMessage ) : null;
			return result;
		}
	}
}