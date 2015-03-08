using DragonSpark.Application.Presentation.Extensions;
using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Presentation.Entity.Security
{
	public class AuthenticationStatusMonitor : Behavior<FrameworkElement>
	{
		public event EventHandler Refreshed = delegate { };

		protected override void OnAttached()
		{
			AssociatedObject.EnsureLoaded( x => HtmlPage.RegisterScriptableObject( GetType().Name, this ) );
			
			base.OnAttached();
		}

		[ScriptableMember]
		public void Refresh()
		{
			DragonSpark.Runtime.Logging.TryAndHandle( () => Refreshed( this, EventArgs.Empty ) );
		}
	}
}
