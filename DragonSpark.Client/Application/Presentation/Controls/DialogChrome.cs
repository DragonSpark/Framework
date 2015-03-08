using System;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Controls
{
	public partial class DialogChrome
	{
		protected override void OnOpened()
		{
			Dispatcher.BeginInvoke( Open );
		}

		void Open()
		{
			this.CenterInScreen();
			base.OnOpened();
		}

		/*[Dependency]
		public INavigationNodeMap NodeMap { get; set; }*/

		public bool Navigate( Uri source )
		{
			this.BuildUpOnce();

			/*var result = !source.IsAbsoluteUri && NodeMap.GetHierarchy( source ).Any();
			
			result.IsTrue( () =>
			               	{
			               		NodeMap.Navigate( source );
			               		Close();
			               	} );

			return result;*/
			return false;
		}
	}
}