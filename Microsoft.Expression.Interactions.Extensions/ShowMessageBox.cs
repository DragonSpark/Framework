// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactions.Extensions {


	/// <summary>
	/// Displays a message box.
	/// </summary>
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "Click" }),
		DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "MouseLeftButtonDown" })]
	public class ShowMessageBox: TriggerAction<FrameworkElement> {

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(ShowMessageBox), new PropertyMetadata(null));
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(ShowMessageBox), new PropertyMetadata(null));
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty MessageBoxButtonProperty = DependencyProperty.Register("MessageBoxButton", typeof(MessageBoxButton), typeof(ShowMessageBox), new PropertyMetadata(MessageBoxButton.OK));

		/// <summary>
		/// Message to be displayed in the message box.
		/// </summary>
		public string Message {
			get { return (string)this.GetValue(ShowMessageBox.MessageProperty); }
			set { this.SetValue(ShowMessageBox.MessageProperty, value); }
		}

		/// <summary>
		/// Caption for the message box.
		/// </summary>
		public string Caption {
			get { return (string)this.GetValue(ShowMessageBox.CaptionProperty); }
			set { this.SetValue(ShowMessageBox.CaptionProperty, value); }
		}

		/// <summary>
		/// Type of messagebox confirmation.
		/// </summary>
		public MessageBoxButton MessageBoxButton {
			get { return (MessageBoxButton)this.GetValue(ShowMessageBox.MessageBoxButtonProperty); }
			set { this.SetValue(ShowMessageBox.MessageBoxButtonProperty, value); }
		}

		/// <summary>
		/// Displays the message box.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {

			if (string.IsNullOrEmpty(this.Caption))
				MessageBox.Show(this.Message);
			else
				MessageBox.Show(this.Message, this.Caption, this.MessageBoxButton);
		}
	}
}
