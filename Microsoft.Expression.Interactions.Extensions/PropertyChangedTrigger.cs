// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;

namespace Microsoft.Expression.Interactions.Extensions {

	/// <summary>
	/// Triggers whenever a property changes, regardless of the value it changes to.
	/// </summary>
	public class PropertyChangedTrigger: TriggerBase<FrameworkElement> {

		/// <summary>Backing DP for Binding property</summary>
		public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(Binding), typeof(PropertyChangedTrigger), new PropertyMetadata(PropertyChangedTrigger.HandleBindingChanged));

		private BindingListener listener;

		/// <summary>
		/// Constructor.
		/// </summary>
		public PropertyChangedTrigger() {
			this.listener = new BindingListener(this.HandleBindingValueChanged);
		}

		/// <summary>
		/// Binding used to trigger this.
		/// </summary>
		public Binding Binding {
			get { return (Binding)this.GetValue(PropertyChangedTrigger.BindingProperty); }
			set { this.SetValue(PropertyChangedTrigger.BindingProperty, value); }
		}

		private void HandleBindingValueChanged(object sender, BindingChangedEventArgs e) {
			this.InvokeActions(e.EventArgs);
		}

		/// <summary>
		/// Perform initialization.
		/// </summary>
		protected override void OnAttached() {
			base.OnAttached();

			this.listener.Element = this.AssociatedObject;
		}

		/// <summary>
		/// Perform cleanup.
		/// </summary>
		protected override void OnDetaching() {
			base.OnDetaching();

			this.listener.Element = null;
		}

		private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((PropertyChangedTrigger)sender).OnBindingChanged(e);
		}

		/// <summary>
		/// Notification that the binding property changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBindingChanged(DependencyPropertyChangedEventArgs e) {
			this.listener.Binding = this.Binding;
		}
	}
}
