// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Microsoft.Expression.Interactions.Extensions {

	/// <summary>
	/// Sample action for quick prototyping with Data.
	/// Takes a random item in a ListBox and duplicates it into a random position.
	/// </summary>
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "Click" }),
		DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "MouseLeftButtonDown" })]
	public class ListBoxAddOne : TargetedTriggerAction<ListBox> {


		private Random random = new Random();


		/// <summary>
		/// Implementation of action.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {

			IList list = this.Target.ItemsSource as IList;
			if (list != null) {
				int index = this.random.Next(list.Count);
				int position = this.random.Next(list.Count);

				list.Insert(position, list[index]);
			}
		}
	}

	/// <summary>
	/// Sample action for quick prototyping with Data.
	/// Takes a random item in a ListBox and removes it.
	/// </summary>
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "Click" }),
		DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "MouseLeftButtonDown" })]
	public class ListBoxRemoveOne : TargetedTriggerAction<ListBox> {


		private Random random = new Random();

		/// <summary>
		/// Removes the item from the ListBox.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {

			IList list = this.Target.ItemsSource as IList;
			if (list != null) {
				list.RemoveAt(this.random.Next(list.Count));
			}
		}
	}

	/// <summary>
	/// Sample action for quick prototyping with Data.
	/// For use inside of a ListBoxItem, will remove this item (DataContext) from the ListBox.
	/// </summary>
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "Click" }),
		DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "MouseLeftButtonDown" })]
	public class ListBoxRemoveThisItem : TargetedTriggerAction<FrameworkElement> {

		/// <summary>
		/// Removes the item from the ListBox.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter) {

			ItemsControl items = this.ItemsControl;
			if (items != null) {
				IList list = items.ItemsSource as IList;
				if (list != null && list.Contains(this.Target.DataContext)) {
					list.Remove(this.Target.DataContext);

				}
			}
		}

		private ItemsControl ItemsControl {
			get {
				FrameworkElement element = VisualTreeHelper.GetParent(this.Target) as FrameworkElement;
				while (element != null) {
					ItemsControl itemsControl = element as ItemsControl;
					if (itemsControl != null)
						return itemsControl;

					element = VisualTreeHelper.GetParent(element) as FrameworkElement;
				}
				return null;
			}
		}
	}
}
