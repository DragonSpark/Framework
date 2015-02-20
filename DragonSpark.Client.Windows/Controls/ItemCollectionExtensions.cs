using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragonSpark.Client.Windows.Controls
{
	/// <summary>
	/// A static class providing methods for working with the visual tree using generics.  
	/// </summary>
	public static class TemplatedVisualTreeExtensions
	{

		#region GetFirstLogicalChildByType<T>(...)
		/// <summary>
		/// Retrieves the first logical child of a specified type using a 
		/// breadth-first search.  A visual element is assumed to be a logical 
		/// child of another visual element if they are in the same namescope.
		/// For performance reasons this method manually manages the queue 
		/// instead of using recursion.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <param name="applyTemplates">Specifies whether to apply templates on the traversed framework elements</param>
		/// <returns>The first logical child of the framework element of the specified type.</returns>
		internal static T GetFirstLogicalChildByType<T>(this FrameworkElement parent, bool applyTemplates)
			where T : FrameworkElement
		{
			Debug.Assert(parent != null, "The parent cannot be null.");

			Queue<FrameworkElement> queue = new Queue<FrameworkElement>();
			queue.Enqueue(parent);

			while (queue.Count > 0)
			{
				FrameworkElement element = queue.Dequeue();
				var elementAsControl = element as Control;
				if (applyTemplates && elementAsControl != null)
				{
					elementAsControl.ApplyTemplate();
				}

				if (element is T && element != parent)
				{
					return (T)element;
				}

				foreach (FrameworkElement visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
				{
					queue.Enqueue(visualChild);
				}
			}

			return null;
		}
		#endregion

		#region GetLogicalChildrenByType<T>(...)
		/// <summary>
		/// Retrieves all the logical children of a specified type using a 
		/// breadth-first search.  A visual element is assumed to be a logical 
		/// child of another visual element if they are in the same namescope.
		/// For performance reasons this method manually manages the queue 
		/// instead of using recursion.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <param name="applyTemplates">Specifies whether to apply templates on the traversed framework elements</param>
		/// <returns>The logical children of the framework element of the specified type.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification="File is linked to projects that target previous platforms that require this method.")]
		internal static IEnumerable<T> GetLogicalChildrenByType<T>(this FrameworkElement parent, bool applyTemplates)
				where T : FrameworkElement
		{
			Debug.Assert(parent != null, "The parent cannot be null.");

			if (applyTemplates && parent is Control)
			{
				((Control)parent).ApplyTemplate();
			}

			Queue<FrameworkElement> queue =
			   new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

			while (queue.Count > 0)
			{
				FrameworkElement element = queue.Dequeue();
				if (applyTemplates && element is Control)
				{
					((Control)element).ApplyTemplate();
				}

				if (element is T)
				{
					yield return (T)element;
				}

				foreach (FrameworkElement visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
				{
					queue.Enqueue(visualChild);
				}
			}
		}
		#endregion
	}

	/// <summary>
	/// A static class providing methods for working with the visual tree.  
	/// </summary>
	internal static class VisualTreeExtensions
	{
		/// <summary>
		/// Retrieves all the visual children of a framework element.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <returns>The visual children of the framework element.</returns>
		internal static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
		{
			Debug.Assert(parent != null, "The parent cannot be null.");

			int childCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int counter = 0; counter < childCount; counter++)
			{
				yield return VisualTreeHelper.GetChild(parent, counter);
			}
		}

		/// <summary>
		/// Retrieves all the logical children of a framework element using a 
		/// breadth-first search.  A visual element is assumed to be a logical 
		/// child of another visual element if they are in the same namescope.
		/// For performance reasons this method manually manages the queue 
		/// instead of using recursion.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <returns>The logical children of the framework element.</returns>
		internal static IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(this FrameworkElement parent)
		{
			Debug.Assert(parent != null, "The parent cannot be null.");

			Queue<FrameworkElement> queue =
				new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

			while (queue.Count > 0)
			{
				FrameworkElement element = queue.Dequeue();
				yield return element;

				foreach (FrameworkElement visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
				{
					queue.Enqueue(visualChild);
				}
			}
		}

		/// <summary>
		/// Gets the ancestors of the element, up to the root, limiting the 
		/// ancestors by FrameworkElement.
		/// </summary>
		/// <param name="node">The element to start from.</param>
		/// <returns>An enumerator of the ancestors.</returns>
		internal static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
		{
			FrameworkElement parent = node.GetVisualParent();
			while (parent != null)
			{
				yield return parent;
				parent = parent.GetVisualParent();
			}
		}

		/// <summary>
		/// Gets the visual parent of the element.
		/// </summary>
		/// <param name="node">The element to check.</param>
		/// <returns>The visual parent.</returns>
		internal static FrameworkElement GetVisualParent(this FrameworkElement node)
		{
			return VisualTreeHelper.GetParent(node) as FrameworkElement;
		}

		/// <summary>
		/// The first parent of the framework element of the specified type 
		/// that is found while traversing the visual tree upwards.
		/// </summary>
		/// <typeparam name="T">
		/// The element type of the dependency object.
		/// </typeparam>
		/// <param name="element">The dependency object element.</param>
		/// <returns>
		/// The first parent of the framework element of the specified type.
		/// </returns>
		internal static T GetParentByType<T>(this DependencyObject element)
			where T : FrameworkElement
		{
			Debug.Assert(element != null, "The element cannot be null.");

			T result = null;
			DependencyObject parent = VisualTreeHelper.GetParent(element);

			while (parent != null)
			{
				result = parent as T;

				if (result != null)
				{
					return result;
				}

				parent = VisualTreeHelper.GetParent(parent);
			}

			return null;
		}
	}

	public static class ItemCollectionExtensions
	{
		public static FrameworkElement GetItem(this ItemCollection items, int index)
		{
			if ((index >= 0) && (index < items.Count))
				return (FrameworkElement)items[index];

			return null;
		}

		public static FrameworkElement GetLastItem(this ItemCollection items)
		{
			if (items.Count == 0)
				return null;

			return (FrameworkElement)items[items.Count - 1];
		}

		public static int GetIndexOfPosition(this ItemCollection items, double position)
		{
			if (items.Count == 0)
				return -1;

			// far left : back to last item
			if (position < 0)
				return items.Count - 1;

			double start = 0.0;
			for (int i = 0; i < items.Count; i++)
			{
				FrameworkElement item = (FrameworkElement)items[i];
				if ((position >= start) && (position < start + item.Width))
					return i;

				start += item.Width;
			}

			// far right : assume first
			return 0;
		}

		public static double GetItemPosition(this ItemCollection items, int index)
		{
			double position = 0.0;
			if ((index >= 0) && (index < items.Count))
			{
				for (int i = 0; i != index; i++)
				{
					FrameworkElement item = items.GetItem(i);
					if (null != item)
						position += item.Width;
				}
			}

			return position;
		}

		public static double GetLastItemPosition(this ItemCollection items)
		{
			return items.GetItemPosition(items.Count - 1);
		}

		public static double GetItemWidth(this ItemCollection items, int index)
		{
			FrameworkElement item = items.GetItem(index);
			if (null != item)
				return item.Width;

			return 0.0;
		}

		public static double GetTotalWidth(this ItemCollection items)
		{
			FrameworkElement item = items.GetLastItem();
			if (null == item)
				return 0.0;

			return items.GetLastItemPosition() + item.Width;
		}
	}
}
