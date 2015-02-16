using System;
using System.Windows.Input;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class EntryCellPhoneTextBox : System.Windows.Controls.TextBox
	{
		public event EventHandler KeyboardReturnPressed = delegate {};
		protected override void OnKeyUp(KeyEventArgs e)
		{
			switch ( e.Key )
			{
				case Key.Enter:
					KeyboardReturnPressed(this, EventArgs.Empty);
					break;
			}
			base.OnKeyUp(e);
		}
	}
}
