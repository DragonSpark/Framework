using System;

namespace DragonSpark.Application.Presentation.Controls
{
	public class ChildWindow : System.Windows.Controls.ChildWindow
	{
		public event EventHandler Opened = delegate { };

		public ChildWindow()
		{}

		protected override void OnOpened()
		{
			Opened( this, EventArgs.Empty );
			Dispatcher.BeginInvoke( Open );
		}

		void Open()
		{
			this.CenterInScreen();
			base.OnOpened();
		}
	}
}