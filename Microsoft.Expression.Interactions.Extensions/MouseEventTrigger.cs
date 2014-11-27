// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Microsoft.Expression.Interactions.Extensions {


	/// <summary>
	/// Trigger to allow more specialized triggering of mouse events
	/// Such as handling modifier keys and double click events.
	/// </summary>
	public class MouseEventTrigger: TargetedTriggerBase<FrameworkElement> {

		/// <summary>Backing DP for DoubleClickTime property</summary>
		public static readonly DependencyProperty DoubleClickTimeProperty = DependencyProperty.Register("DoubleClickTime", typeof(TimeSpan), typeof(MouseEventTrigger), new PropertyMetadata(TimeSpan.FromSeconds(.5)));
		/// <summary>Backing DP for ClickCount property</summary>
		public static readonly DependencyProperty ClickCountProperty = DependencyProperty.Register("ClickCount", typeof(int), typeof(MouseEventTrigger), new PropertyMetadata(2));
		/// <summary>Backing DP for ProcessHandledEvents property</summary>
		public static readonly DependencyProperty ProcessHandledEventsProperty = DependencyProperty.Register("ProcessHandledEvents", typeof(bool), typeof(MouseEventTrigger), new PropertyMetadata(false));
		/// <summary>Backing DP for Modifiers property</summary>
		public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(MouseEventTrigger), new PropertyMetadata(ModifierKeys.None));
		/// <summary>Backing DP for ClickMode property</summary>
		public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(MouseEventTrigger), new PropertyMetadata(ClickMode.Press));

	

		DateTime? lastMouseDownTime = null;
		Point lastMouseDownLocation;
		private int currentClickCount;

		/// <summary>
		/// Maximum time between clicks to count as a double-click.
		/// </summary>
		public TimeSpan DoubleClickTime {
			get { return (TimeSpan)this.GetValue(MouseEventTrigger.DoubleClickTimeProperty); }
			set { this.SetValue(MouseEventTrigger.DoubleClickTimeProperty, value); }
		}

		/// <summary>
		/// True if this should trigger off of already handled events.
		/// </summary>
		public bool ProcessHandledEvents {
			get { return (bool)this.GetValue(MouseEventTrigger.ProcessHandledEventsProperty); }
			set { this.SetValue(MouseEventTrigger.ProcessHandledEventsProperty, value); }
		}

		/// <summary>
		/// Number of times the mouse needs to be clicked to trigger this.
		/// For double-click events, this would be 2.
		/// </summary>
		public int ClickCount {
			get { return (int)this.GetValue(MouseEventTrigger.ClickCountProperty); }
			set { this.SetValue(MouseEventTrigger.ClickCountProperty, value); }
		}

		/// <summary>
		/// Any modifier keys which should be active to trigger this.
		/// </summary>
		public ModifierKeys Modifiers {
			get { return (ModifierKeys)this.GetValue(MouseEventTrigger.ModifiersProperty); }
			set { this.SetValue(MouseEventTrigger.ModifiersProperty, value); }
		}

		/// <summary>
		/// Mode which this is triggering
		/// </summary>
		public ClickMode ClickMode {
			get { return (ClickMode)this.GetValue(MouseEventTrigger.ClickModeProperty); }
			set { this.SetValue(MouseEventTrigger.ClickModeProperty, value); }
		}

		/// <summary>
		/// Hook up event handlers.
		/// </summary>
		protected override void OnSourceAttached() {
			base.OnSourceAttached();

			switch (this.ClickMode) {
				case ClickMode.Hover:
					this.Source.MouseEnter += this.HandleMouseEnter;
					break;
				case ClickMode.Press:
					this.Source.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, (MouseButtonEventHandler)this.HandleMouseDown, this.ShouldProcessHandledEvents);
					break;
				case ClickMode.Release:
					this.Source.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, (MouseButtonEventHandler)this.HandleMouseDown, this.ShouldProcessHandledEvents);
					break;
			}
		}

		/// <summary>
		/// Clean up event handlers.
		/// </summary>
		protected override void OnSourceDetaching() {
			base.OnSourceDetaching();

			switch (this.ClickMode) {
				case ClickMode.Hover:
					this.Source.MouseEnter -= this.HandleMouseEnter;
					break;
				case ClickMode.Press:
					this.Source.RemoveHandler(FrameworkElement.MouseLeftButtonDownEvent, (MouseButtonEventHandler)this.HandleMouseDown);
					break;
				case ClickMode.Release:
					this.Source.RemoveHandler(FrameworkElement.MouseLeftButtonUpEvent, (MouseButtonEventHandler)this.HandleMouseDown);
					break;
			}
		}

		private void HandleMouseDown(object sender, MouseButtonEventArgs e) {
			if (Keyboard.Modifiers == this.Modifiers) {
				if (this.lastMouseDownTime == null
					|| DateTime.Now - this.lastMouseDownTime > this.DoubleClickTime
					|| e.GetPosition(this.Source) != this.lastMouseDownLocation) {

					this.lastMouseDownTime = DateTime.Now;
					this.lastMouseDownLocation = e.GetPosition(this.Source);

					this.currentClickCount = 1;
				}
				else {
					++this.currentClickCount;
				}

				if (this.currentClickCount == this.ClickCount)
					this.InvokeActions(e);

				e.Handled = true;
			}
		}

		private void HandleMouseEnter(object sender, MouseEventArgs e) {
			if (Keyboard.Modifiers == this.Modifiers) {
				if (this.lastMouseDownTime == null
					|| DateTime.Now - this.lastMouseDownTime > this.DoubleClickTime) {

					this.lastMouseDownTime = DateTime.Now;

					this.currentClickCount = 1;
				}
				else {
					++this.currentClickCount;
				}

				if (this.currentClickCount == this.ClickCount)
					this.InvokeActions(e);
			}
		}

		/// <summary>
		/// Checks to see if processed events should be handled as well.
		/// </summary>
		protected virtual bool ShouldProcessHandledEvents {
			get {
				// Handle all events from Buttons if the user hasn't specified a value.
				if (this.ReadLocalValue(MouseEventTrigger.ProcessHandledEventsProperty) == DependencyProperty.UnsetValue)
					return typeof(ButtonBase).IsAssignableFrom(this.Source.GetType());

				return this.ProcessHandledEvents;
			}
		}


	}
}
