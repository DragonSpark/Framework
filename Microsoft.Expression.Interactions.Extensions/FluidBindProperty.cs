// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;

namespace Microsoft.Expression.Interactions.Extensions {

	/// <summary>
	/// Allows animating the changes from databound properties
	/// </summary>
	public class FluidBindProperty: Behavior<FrameworkElement> {

		/// <summary>Backing DP for the Binding property</summary>
		public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(Binding), typeof(FluidBindProperty), new PropertyMetadata(FluidBindProperty.HandleBindingChanged));
		/// <summary>Backing DP for the Duration property</summary>
		public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(FluidBindProperty), new PropertyMetadata(new Duration(TimeSpan.FromSeconds(.25))));
		/// <summary>Backing DP for the PropertyName property</summary>
		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(FluidBindProperty), new PropertyMetadata(null));

		private BindingListener listener;

		/// <summary>
		/// Constructor
		/// </summary>
		public FluidBindProperty() {
			this.listener = new BindingListener(this.HandleBindingValueChanged);
		}

		/// <summary>
		/// Binding to the value which is to be animated to.
		/// </summary>
		public Binding Binding {
			get { return (Binding)this.GetValue(FluidBindProperty.BindingProperty); }
			set { this.SetValue(FluidBindProperty.BindingProperty, value); }
		}

#if SILVERLIGHT
		/// <summary>Backing DP for the Ease property</summary>
		public static readonly DependencyProperty EaseProperty = DependencyProperty.Register("Ease", typeof(IEasingFunction), typeof(FluidBindProperty), new PropertyMetadata(null));
		/// <summary>
		/// Easing function used to animate to the value.
		/// </summary>
		public IEasingFunction Ease {
			get { return (IEasingFunction)this.GetValue(FluidBindProperty.EaseProperty); }
			set { this.SetValue(FluidBindProperty.EaseProperty, value); }
		}
#endif
		/// <summary>
		/// Duration of the animation
		/// </summary>
		public Duration Duration {
			get { return (Duration)this.GetValue(FluidBindProperty.DurationProperty); }
			set { this.SetValue(FluidBindProperty.DurationProperty, value); }
		}

		/// <summary>
		/// Name of the property to be set.
		/// </summary>
		public string PropertyName {
			get { return (string)this.GetValue(FluidBindProperty.PropertyNameProperty); }
			set { this.SetValue(FluidBindProperty.PropertyNameProperty, value); }
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

		private void HandleBindingValueChanged(object sender, BindingChangedEventArgs e) {
			object value = this.listener.Value;
			FrameworkElement target = this.AssociatedObject;
#if SILVERLIGHT
			FluidBindProperty.AnimateTo(target, this.PropertyName, value, this.Duration, this.Ease, false);
#else
			FluidBindProperty.AnimateTo(target, this.PropertyName, value, this.Duration, false);
#endif
		}

#if SILVERLIGHT
		internal static void AnimateTo(FrameworkElement target, string propertyName, object value, Duration duration, IEasingFunction ease, bool increment) {
#else
		internal static void AnimateTo(FrameworkElement target, string propertyName, object value, Duration duration, bool increment) {
#endif
			

			if (!string.IsNullOrEmpty(propertyName) && target != null) {
				PropertyInfo property = target.GetType().GetProperty(propertyName);
				if (property == null)
					throw new ArgumentException("Cannot find property " + propertyName + " on object " + target.GetType().Name);

				if (!property.CanWrite)
					throw new ArgumentException("Property is read-only " + propertyName + " on object " + target.GetType().Name);

				object toValue = value;
				TypeConverter typeConverter = ConverterHelper.GetTypeConverter(property.PropertyType);
				Exception innerException = null;
				try {
					if (((typeConverter != null) && (value != null)) && typeConverter.CanConvertFrom(value.GetType()))
						toValue = typeConverter.ConvertFrom(value);

					object fromValue = property.GetValue(target, null);

					if (increment)
						toValue = FluidBindProperty.Add(toValue, fromValue);

					if (duration.HasTimeSpan) {
						Timeline timeline;
						if ((typeof(FrameworkElement).IsAssignableFrom(target.GetType()) && ((propertyName == "Width") || (propertyName == "Height"))) && double.IsNaN((double)fromValue)) {
							if (propertyName == "Width")
								fromValue = target.ActualWidth;
							else
								fromValue = target.ActualHeight;
						}

						Storyboard storyboard = new Storyboard();
						if (typeof(double).IsAssignableFrom(property.PropertyType)) {
							DoubleAnimation animation = new DoubleAnimation();
							animation.From = new double?((double)fromValue);
							animation.To = new double?((double)toValue);
#if SILVERLIGHT
							animation.EasingFunction = ease;
#endif
							timeline = animation;
						}
						else if (typeof(Color).IsAssignableFrom(property.PropertyType)) {
							ColorAnimation animation2 = new ColorAnimation();
							animation2.From = new Color?((Color)fromValue);
							animation2.To = new Color?((Color)toValue);
#if SILVERLIGHT
							animation2.EasingFunction = ease;
#endif
							timeline = animation2;
						}
						else if (typeof(Point).IsAssignableFrom(property.PropertyType)) {
							PointAnimation animation3 = new PointAnimation();
							animation3.From = new Point?((Point)fromValue);
							animation3.To = new Point?((Point)toValue);
#if SILVERLIGHT
							animation3.EasingFunction = ease;
#endif
							timeline = animation3;
						}
						else {
							ObjectAnimationUsingKeyFrames frames = new ObjectAnimationUsingKeyFrames();
							DiscreteObjectKeyFrame frame = new DiscreteObjectKeyFrame();
							frame.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0L));
							frame.Value = fromValue;
							DiscreteObjectKeyFrame frame2 = new DiscreteObjectKeyFrame();
							frame2.KeyTime = KeyTime.FromTimeSpan(duration.TimeSpan);
							frame2.Value = toValue;
							frames.KeyFrames.Add(frame);
							frames.KeyFrames.Add(frame2);
							timeline = frames;
						}
						timeline.Duration = duration;
						storyboard.Children.Add(timeline);
						Storyboard.SetTarget(storyboard, target);
						Storyboard.SetTargetProperty(storyboard, new PropertyPath(property.Name, new object[0]));
						storyboard.Begin();
					}
					else
						property.SetValue(target, toValue, new object[0]);
				}
				catch (FormatException exception2) {
					innerException = exception2;
				}
				catch (ArgumentException exception3) {
					innerException = exception3;
				}
				catch (MethodAccessException exception4) {
					innerException = exception4;
				}
				if (innerException != null)
					throw new ArgumentException(innerException.Message);

			}
		}

		internal static object Add(object a, object b) {
			if (a.GetType() != b.GetType())
				throw new Exception("Types must match");

			Type type = a.GetType();
			if (type == typeof(double))
				return (double)a + (double)b;
			else if (type == typeof(int))
				return (int)a + (int)b;
			else if (type == typeof(string))
				return (string)a + (string)b;
			else if (type == typeof(float))
				return (float)a + (float)b;

			MethodInfo add = type.GetMethod("op_Addition");
			if (add != null)
				return add.Invoke(null, new object[] { a, b });

			throw new Exception("Unable to add type " + type);
		}

		private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((FluidBindProperty)sender).OnBindingChanged(e);
		}

		/// <summary>
		/// Notification that the binding property has changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBindingChanged(DependencyPropertyChangedEventArgs e) {
			this.listener.Binding = this.Binding;
		}
	}
}
