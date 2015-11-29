// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;

namespace Microsoft.Expression.Interactions.Extensions {

	/// <summary>
	/// Invokes a command which is exposed by the data context.
	/// </summary>
	public class InvokeDataCommand: TriggerAction<FrameworkElement> {

		/// <summary>Backing DP for the Command property</summary>
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(Binding), typeof(InvokeDataCommand), new PropertyMetadata(null, InvokeDataCommand.HandleCommandChanged));
		/// <summary>Backing DP for the CommandParameter property</summary>
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeDataCommand), new PropertyMetadata(null));

		private BindingListener listener = new BindingListener();

		/// <summary>
		/// Binding to the command which is to be invoked
		/// </summary>
		public Binding Command {
			get { return (Binding)this.GetValue(InvokeDataCommand.CommandProperty); }
			set { this.SetValue(InvokeDataCommand.CommandProperty, value); }
		}

		/// <summary>
		/// Optional parameter for the command.
		/// </summary>
		public object CommandParameter {
			get { return (object)this.GetValue(InvokeDataCommand.CommandParameterProperty); }
			set { this.SetValue(InvokeDataCommand.CommandParameterProperty, value); }
		}

		/// <summary>
		/// Initialization
		/// </summary>
		protected override void OnAttached() {
			base.OnAttached();

			this.listener.Element = this.AssociatedObject;
		}

		/// <summary>
		/// Cleanup
		/// </summary>
		protected override void OnDetaching() {
			base.OnDetaching();

			this.listener.Element = null;
		}

		/// <summary>
		/// Fire the command.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {

			ICommand command = this.listener.Value as ICommand;
			if (command != null && command.CanExecute(this.CommandParameter)) {
				command.Execute(this.CommandParameter);
			}

		}

		private static void HandleCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((InvokeDataCommand)sender).OnCommandChanged(e);
		}

		/// <summary>
		/// Notification that the command property has changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnCommandChanged(DependencyPropertyChangedEventArgs e) {
			this.listener.Binding = this.Command;
		}
	}
}
