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
	/// Allows triggering actions based on the value of a property on the data context.
	/// </summary>
	public class DataTrigger : TriggerBase<FrameworkElement> {

		private BindingListener listener;
		private object bindingValue;

		/// <summary>Backing DP for the Value property</summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(DataTrigger), new PropertyMetadata(null, DataTrigger.HandleValueChanged));
		/// <summary>Backing DP for the Bindng property</summary>
		public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(Binding), typeof(DataTrigger), new PropertyMetadata(null, DataTrigger.HandleBindingChanged));
		/// <summary>Backing DP for the TriggerOnRising property</summary>
		public static readonly DependencyProperty TriggerOnRisingProperty = DependencyProperty.Register("TriggerOnRising", typeof(bool), typeof(DataTrigger), new PropertyMetadata(true));

		/// <summary>
		/// Constructor
		/// </summary>
		public DataTrigger() {
			this.listener = new BindingListener(this.HandleBindingValueChanged);
		}

		/// <summary>
		/// True if this should invoke the action when the condition goes true,
		/// False if this inokes when the condition goes false.
		/// </summary>
		public bool TriggerOnRising {
			get { return (bool)this.GetValue(DataTrigger.TriggerOnRisingProperty); }
			set { this.SetValue(DataTrigger.TriggerOnRisingProperty, value); }
		}

		/// <summary>
		/// The value to compare against.
		/// </summary>
		public object Value {
			get { return (object)this.GetValue(DataTrigger.ValueProperty); }
			set { this.SetValue(DataTrigger.ValueProperty, value); }
		}

		/// <summary>
		/// Binding to the property on which this should be comparing the Value to.
		/// </summary>
		public Binding Binding {
			get { return (Binding)this.GetValue(DataTrigger.BindingProperty); }
			set { this.SetValue(DataTrigger.BindingProperty, value); }
		}

		/// <summary>
		/// Perform initialization
		/// </summary>
		protected override void OnAttached() {
			base.OnAttached();

			this.listener.Element = this.AssociatedObject;
		}

		/// <summary>
		/// Perform cleanup
		/// </summary>
		protected override void OnDetaching() {
			base.OnDetaching();

			this.listener.Element = null;
		}

		private void HandleBindingValueChanged(object sender, BindingChangedEventArgs e) {
			this.bindingValue = e.EventArgs.NewValue;

			this.CheckState();
		}

		private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((DataTrigger)sender).OnBindingChanged(e);
		}

		/// <summary>
		/// Notification that the binding has changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBindingChanged(DependencyPropertyChangedEventArgs e) {
			this.listener.Binding = this.Binding;
		}


		private static void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((DataTrigger)sender).OnValueChanged(e);
		}

		/// <summary>
		/// Notification that the value has changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e) {
			this.CheckState();
		}

		private void CheckState() {

			if (this.Value == null || this.bindingValue == null) {
				this.IsTrue = object.Equals(this.bindingValue, this.Value);
			}
			else {
				object convertedValue = ConverterHelper.ConvertToType(this.Value, this.bindingValue.GetType());
				this.IsTrue = object.Equals(this.bindingValue, ConverterHelper.ConvertToType(this.Value, this.bindingValue.GetType()));
			}
		}

		private bool isTrue;
		private bool IsTrue {
			get { return this.isTrue; }
			set {
				if (this.isTrue != value) {
					this.isTrue = value;

					if (this.IsTrue == this.TriggerOnRising)
						this.InvokeActions(this.bindingValue);
				}
			}
		}
	}
}
