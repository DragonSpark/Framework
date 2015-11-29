// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Threading;
using System.Windows;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactions.Extensions {
	/// <summary>
	/// Base class for triggers that can target named elements.
	/// </summary>
	public abstract class TargetedTriggerBase: TriggerBase<DependencyObject> {

		/// <summary>Backing DP for SourceName property </summary>
		public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName", typeof(string), typeof(TargetedTriggerBase), new PropertyMetadata(null, TargetedTriggerBase.HandleSourceNameChanged));

		private NameResolver sourceNameResolver;

		/// <summary>
		/// Constructor
		/// </summary>
		public TargetedTriggerBase() {
			this.sourceNameResolver = new NameResolver();
		}

		/// <summary>
		/// Name of the object to be targeted
		/// </summary>
		public string SourceName {
			get { return (string)this.GetValue(TargetedTriggerBase.SourceNameProperty); }
			set { this.SetValue(TargetedTriggerBase.SourceNameProperty, value); }
		}

		/// <summary>
		/// Notification that this is now attached to a source.
		/// </summary>
		protected virtual void OnSourceObjectAttached() {
		}

		/// <summary>
		/// Notification that this is no longer attached to a source.
		/// </summary>
		protected virtual void OnSourceObjectDetaching() {
		}

		/// <summary>
		/// Performs initialization
		/// </summary>
		protected override void OnAttached() {
			base.OnAttached();

			DependencyObject associatedObject = base.AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			FrameworkElement element = associatedObject as FrameworkElement;
			this.RegisterSourceChanged();
			if (behavior != null) {
#if SILVERLIGHT
				this.Dispatcher.BeginInvoke(delegate {
					this.UpdateSourceFromBehavior(behavior);
				});
#else
				this.Dispatcher.BeginInvoke((ThreadStart)delegate {
					this.UpdateSourceFromBehavior(behavior);
				});
#endif
			}
			else if (element != null) {
				this.sourceNameResolver.NameScopeReferenceElement = element;
			}
			else {
				this.UpdateSource(null);
			}

		}

		private void RegisterSourceChanged() {
			if (!this.IsSourceChangedRegistered) {
				this.sourceNameResolver.ResolvedElementChanged += this.HandleResolvedElementChanged;
				this.IsSourceChangedRegistered = true;
			}
		}

		private bool IsSourceChangedRegistered {
			get;
			set;
		}

		private void UpdateSourceFromBehavior(IAttachedObject obj) {
			this.sourceNameResolver.NameScopeReferenceElement = ((IAttachedObject)obj).AssociatedObject as FrameworkElement;
		}

		private void HandleResolvedElementChanged(object sender, NameResolvedEventArgs e) {
			this.UpdateSource(e.NewObject);
		}

		private static void HandleSourceNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((TargetedTriggerBase)sender).OnSourceNameChanged(e);
		}

		private void UpdateSource(object newSource) {
			if (newSource != this.SourceObject) {
				if (this.SourceObject != null)
					this.OnSourceObjectDetaching();

				this.SourceObject = newSource;
				if (this.SourceObject != null)
					this.OnSourceObjectAttached();
			}
		}

		/// <summary>
		/// Notification that the source name property has changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSourceNameChanged(DependencyPropertyChangedEventArgs e) {
			this.sourceNameResolver.Name = this.SourceName;
		}

		/// <summary>
		/// Object which this is targeting.
		/// </summary>
		protected object SourceObject {
			get;
			private set;
		}
	}

	/// <summary>
	/// TargetedTriggerBase which targets specific types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class TargetedTriggerBase<T> : TargetedTriggerBase where T: class {

		/// <summary>
		/// The object which this is targeting.
		/// </summary>
		protected T Source {
			get;
			private set;
		}

		/// <summary>
		/// Notification that this is now attached to a source.
		/// </summary>
		protected virtual void OnSourceAttached() {
		}

		/// <summary>
		/// Notification that this is no longer attached to a source.
		/// </summary>
		protected virtual void OnSourceDetaching() {
		}

		/// <summary>
		/// Performs initialization.
		/// </summary>
		protected override void OnSourceObjectAttached() {
			base.OnSourceObjectAttached();

			this.UpdateSource(this.SourceObject as T);
		}

		private void UpdateSource(T newSource) {
			if (!object.Equals(newSource, this.Source)) {
				if (this.Source != null)
					this.OnSourceDetaching();

				this.Source = newSource;
				if (this.SourceObject != null)
					this.OnSourceAttached();
			}
		}

		/// <summary>
		/// Performs cleanup.
		/// </summary>
		protected override void OnSourceObjectDetaching() {
			base.OnSourceObjectDetaching();

			if (this.Source != null)
				this.OnSourceDetaching();
			this.Source = null;
		}
	}
}
