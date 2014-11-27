// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Threading;
using Microsoft.Expression.Interactivity.Core;

namespace Microsoft.Expression.Interactions.Extensions {
	
	/// <summary>
	/// Base class for a number of GoToState triggers.
	/// </summary>
	public abstract class GoToStateBase : TargetedTriggerAction<FrameworkElement> {

		private int currentStateIndex = 0;

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(GoToStateBase), new PropertyMetadata(true));

		/// <summary>
		/// True if transitions should be used for the state change.
		/// </summary>
		public bool UseTransitions {
			get { return (bool)this.GetValue(GoToStateBase.UseTransitionsProperty); }
			set { this.SetValue(GoToStateBase.UseTransitionsProperty, value); }
		}

		/// <summary>
		/// Hooks up necessary handlers for the state changes.
		/// </summary>
		protected override void OnAttached() {
			base.OnAttached();

			FrameworkElement element = this.TargetElement;
			if (element != null) {
				foreach (VisualStateGroup vsg in this.TargetVSM) {
					vsg.CurrentStateChanged += this.HandleStateChanged;
				}
			}
			else {
#if SILVERLIGHT
				this.Dispatcher.BeginInvoke(delegate {
					this.OnAttached();
				});
#else
				this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)delegate {
					this.OnAttached();
				});
#endif
			}
		}

		/// <summary>
		/// Cleans up when getting removed.
		/// </summary>
		protected override void OnDetaching() {
			base.OnDetaching();

			FrameworkElement element = this.TargetElement;
			if (element != null) {
				foreach (VisualStateGroup vsg in this.TargetVSM) {
					vsg.CurrentStateChanged -= this.HandleStateChanged;
				}
			}
		}

		private void HandleStateChanged(object sender, VisualStateChangedEventArgs e) {
			int index = 0;
			foreach (VisualStateGroup vsg in this.TargetVSM) {
				foreach (VisualState state in vsg.States) {
					if (state == e.NewState) {
						this.currentStateIndex = index;
					}
					++index;
				}
			}
		}

		/// <summary>
		/// The total number of states in the targetted VSM.
		/// </summary>
		protected int StateCount {
			get {
				int count = 0;
				foreach (VisualStateGroup vsg in this.TargetVSM)
					foreach (VisualState state in vsg.States)
						++count;
				return count;
			}
		}

		/// <summary>
		/// The index of the current state in the targetted VSM.
		/// </summary>
		protected int CurrentStateIndex {
			get { return this.currentStateIndex; }
		}

		/// <summary>
		/// Goes to the specified state on the targetted VSM.
		/// </summary>
		/// <param name="targetIndex">index of the state to be activated</param>
		/// <param name="useTransitions">True if transitions should be used.</param>
		protected void GoToState(int targetIndex, bool useTransitions) {

			FrameworkElement targetElement = this.TargetElement;
			if (targetElement == null)
				return;
			
			int index = 0;
			foreach (VisualStateGroup vsg in this.TargetVSM) {
				foreach (VisualState state in vsg.States) {
					if (index == targetIndex) {
						GoToStateBase.GoToState(targetElement, state.Name, useTransitions);
						break;
					}
					++index;
				}
			}
		}

		/// <summary>
		/// Get the control which this action should be changing the state of.
		/// The control is always the parent in the logical tree which contains a VSM. If this
		/// action is attached to a button in UserControl.xaml, then the state group should be the
		/// UserControl's state group- not the Button's.
		/// </summary>
		private FrameworkElement TargetElement {
			get {
				return GoToStateBase.FindTargetElement(this.Target);
			}
		}

		private IList<VisualStateGroup> TargetVSM {
			get {
				return GoToStateBase.FindVSM(this.Target);
			}
		}

		internal static FrameworkElement FindTargetElement(FrameworkElement element) {
			FrameworkElement parent = element;

			while (parent != null) {
				IList vsgs = VisualStateManager.GetVisualStateGroups(parent);
				if (vsgs != null && vsgs.Count > 0) {
					Control control = parent.Parent as Control;
					if (control != null)
						return control;
					return parent;
				}
				parent = parent.Parent as FrameworkElement;
			}

			return null;
		}

		internal static IList<VisualStateGroup> FindVSM(FrameworkElement element) {
			FrameworkElement parent = element;
			bool foundVSM = false;

			List<VisualStateGroup> stateGroups = new List<VisualStateGroup>();

			while (parent != null) {
				if (!foundVSM) {
					IList vsgs = VisualStateManager.GetVisualStateGroups(parent);
					if (vsgs != null && vsgs.Count > 0) {
						foreach (VisualStateGroup vsg in vsgs)
							stateGroups.Add(vsg);
						return stateGroups;
					}
				}
				parent = parent.Parent as FrameworkElement;
			}

			return stateGroups;
		}

		internal static bool GoToState(FrameworkElement element, string stateName, bool useTransitions) {
			var control = element as Control;
			if ( control != null )
			{
				control.ApplyTemplate();
				var goToState = VisualStateManager.GoToState( control, stateName, useTransitions ) || VisualStateManager.GoToElementState( element, stateName, useTransitions );
				return goToState;
			}
			return VisualStateManager.GoToElementState( element, stateName, useTransitions );
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty LoopProperty = DependencyProperty.Register("Loop", typeof(bool), typeof(GoToStateBase), new PropertyMetadata(false));

		/// <summary>
		/// True if this should loop around when it gets to the end.
		/// </summary>
		public bool Loop {
			get { return (bool)this.GetValue(GoToStateBase.LoopProperty); }
			set { this.SetValue(GoToStateBase.LoopProperty, value); }
		}

	}

	/// <summary>
	/// Go to the next state in the targetted VSM.
	/// </summary>
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "Click" }),
		DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "MouseLeftButtonDown" })]
	public class GoToNextState : GoToStateBase {

		/// <summary>
		/// Does the state transition.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {

			int index = this.CurrentStateIndex + 1;
			if (index >= this.StateCount && this.Loop)
				index = 0;

			this.GoToState(index, this.UseTransitions);
		}
	}

	/// <summary>
	/// Go to the previous state in the targetted VSM.
	/// </summary>
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "Click" }),
		DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "MouseLeftButtonDown" })]
	public class GoToPreviousState : GoToStateBase {

		/// <summary>
		/// Does the state transition.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {
			int index = this.CurrentStateIndex - 1;
			if (index < 0 && this.Loop)
				index = this.StateCount - 1;

			this.GoToState(index, this.UseTransitions);
		}
	}

}
