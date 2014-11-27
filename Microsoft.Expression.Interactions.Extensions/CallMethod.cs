// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactions.Extensions {
	/// <summary>
	/// An action that invokes a method on the targeted object.
	/// </summary>
	[DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), "MouseLeftButtonDown")]
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), "Click")]
	public class CallMethod : TargetedTriggerAction<DependencyObject> {

		/// <summary>Backing DP for the MethodName property</summary>
		public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register("MethodName", typeof(string), typeof(CallMethod),
#if SILVERLIGHT
																									new PropertyMetadata(new PropertyChangedCallback(OnMethodNameChanged)));
#else
																									new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMethodNameChanged)));
#endif
		private MethodInfo methodInfo;

		/// <summary>
		/// The name of method to invoke.  It is expected that this method takes no arguments.  This is a dependency property.
		/// </summary>
		public string MethodName {
			get { return (string)this.GetValue(MethodNameProperty); }
			set { this.SetValue(MethodNameProperty, value); }
		}

		private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {
			CallMethod callMethodAction = (CallMethod)sender;
			callMethodAction.UpdateMethodInfo();
		}

		/// <summary>
		/// Implementation of Invoke
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {
			if (this.methodInfo != null) {
				ParameterInfo[] parameters = this.methodInfo.GetParameters();
				if (parameters.Length == 0)
					this.methodInfo.Invoke(this.Target, null);
				else if (parameters.Length == 2 && this.AssociatedObject != null && parameter != null) {
					if (parameters[0].ParameterType.IsAssignableFrom(this.AssociatedObject.GetType())
						&& parameters[1].ParameterType.IsAssignableFrom(parameter.GetType())) {

						this.methodInfo.Invoke(this.Target, new object[] { this.AssociatedObject, parameter });
					}
				}
			}
		}

		/// <summary>
		/// Update the method info when the target changes.
		/// </summary>
		/// <param name="oldTarget"></param>
		/// <param name="newTarget"></param>
		protected override void OnTargetChanged(DependencyObject oldTarget, DependencyObject newTarget) {
			base.OnTargetChanged(oldTarget, newTarget);
			this.UpdateMethodInfo();
		}

		private void UpdateMethodInfo() {
			if (this.TargetObject != null && !string.IsNullOrEmpty(this.MethodName)) {
				Type targetType = this.Target.GetType();
				// Bug 77393: Look for a method that has no parameters to avoid ambiguous method exception
				MethodInfo methodInfo = targetType.GetMethod(this.MethodName, Type.EmptyTypes);
				if (methodInfo == null) {
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
						ExceptionStringTable.CallMethodActionMethodDoesNotExistMessage, this.MethodName, targetType.Name));
				}
				else if (methodInfo.GetParameters().Length != 0) {
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.CallMethodActionZeroParametersOnlyMessage));
				}
				this.methodInfo = methodInfo;
			}
			else {
				this.methodInfo = null;
			}
		}
	}
}
