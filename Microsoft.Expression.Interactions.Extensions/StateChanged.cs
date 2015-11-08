// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Microsoft.Expression.Interactions.Extensions {

	/// <summary>
	/// Triggers when the state is changed.
	/// </summary>
	public class StateChanged : TriggerBase<FrameworkElement> {

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register("StateName", typeof(string), typeof(StateChanged), new PropertyMetadata(null));
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty CompletedProperty = DependencyProperty.Register("Completed", typeof(bool), typeof(StateChanged), new PropertyMetadata(false));

		/// <summary>
		/// True if this should be triggering after the state change has completed,
		/// False if it triggers when the state change starts.
		/// </summary>
		public bool Completed {
			get { return (bool)this.GetValue(StateChanged.CompletedProperty); }
			set { this.SetValue(StateChanged.CompletedProperty, value); }
		}



		private IList<VisualStateGroup> vsgs;

		/// <summary>
		/// Name of the state to be triggered on.
		/// If this is null, then it will trigger on all state changes.
		/// </summary>
		public string StateName {
			get { return (string)this.GetValue(StateChanged.StateNameProperty); }
			set { this.SetValue(StateChanged.StateNameProperty, value); }
		}

		/// <summary>
		/// Hooks up to the necessary events on the VSM.
		/// </summary>
		protected override void OnAttached() {
			base.OnAttached();
#if SILVERLIGHT
			this.Dispatcher.BeginInvoke(delegate {
				this.vsgs = GoToStateBase.FindVSM(this.AssociatedObject);

				foreach (VisualStateGroup vsg in this.vsgs) {
					if (this.Completed)
						vsg.CurrentStateChanged += this.HandleStateChanged;
					else
						vsg.CurrentStateChanging += this.HandleStateChanged;
				}
			});
#else
			this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)delegate {
				this.vsgs = GoToStateBase.FindVSM(this.AssociatedObject);

				foreach (VisualStateGroup vsg in this.vsgs) {
					if (this.Completed)
						vsg.CurrentStateChanged += this.HandleStateChanged;
					else
						vsg.CurrentStateChanging += this.HandleStateChanged;
				}
			});
#endif
		}

		/// <summary>
		/// Cleans up when getting removed.
		/// </summary>
		protected override void OnDetaching() {
			base.OnDetaching();

			foreach (VisualStateGroup vsg in this.vsgs) {
				if (this.Completed)
					vsg.CurrentStateChanged -= this.HandleStateChanged;
				else
					vsg.CurrentStateChanging -= this.HandleStateChanged;
			}
		}

		private void HandleStateChanged(object sender, VisualStateChangedEventArgs e) {
			if (string.IsNullOrEmpty(this.StateName) || string.Compare(this.StateName, e.NewState.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
				this.InvokeActions(e);
		}

	}

}
