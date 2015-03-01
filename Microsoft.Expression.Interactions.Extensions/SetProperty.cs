// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;

namespace Microsoft.Expression.Interactions.Extensions {

	/// <summary>
	/// Sets or increments a property when triggered.
	/// </summary>
	[ContentProperty( "Value" )]
	public class SetProperty: TargetedTriggerAction<DependencyObject> {

		/// <summary>
		/// Backing DP for Value property
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(SetProperty), new PropertyMetadata(null));

		/// <summary>
		/// The value to be set
		/// </summary>
		public object Value {
			get { return (object)this.GetValue(SetProperty.ValueProperty); }
			set { this.SetValue(SetProperty.ValueProperty, value); }
		}

		/// <summary>
		/// Backing DP for the PropertyName property
		/// </summary>
		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(SetProperty), new PropertyMetadata(null));

		/// <summary>
		/// Name of the property to be set.
		/// </summary>
		public string PropertyName {
			get { return (string)this.GetValue(SetProperty.PropertyNameProperty); }
			set { this.SetValue(SetProperty.PropertyNameProperty, value); }
		}
#if SILVERLIGHT
		/// <summary>
		/// Backing DP for the Ease property
		/// </summary>
		public static readonly DependencyProperty EaseProperty = DependencyProperty.Register("Ease", typeof(IEasingFunction), typeof(SetProperty), new PropertyMetadata(null));

		/// <summary>
		/// Easing function to be used for the animation.
		/// </summary>
		public IEasingFunction Ease {
			get { return (IEasingFunction)this.GetValue(SetProperty.EaseProperty); }
			set { this.SetValue(SetProperty.EaseProperty, value); }
		}
#endif
		/// <summary>
		/// Backing DP for the Duration property
		/// </summary>
		public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(SetProperty), new PropertyMetadata(System.Windows.Duration.Automatic));

		/// <summary>
		/// Duration of the animation for the transition
		/// </summary>
		public Duration Duration {
			get { return (Duration)this.GetValue(SetProperty.DurationProperty); }
			set { this.SetValue(SetProperty.DurationProperty, value); }
		}

		/// <summary>
		/// Backing DP for the Increment property
		/// </summary>
		public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(bool), typeof(SetProperty), new PropertyMetadata(null));

		/// <summary>
		/// True if the value is added to the current value rather than directly set.
		/// </summary>
		public bool Increment {
			get { return (bool)this.GetValue(SetProperty.IncrementProperty); }
			set { this.SetValue(SetProperty.IncrementProperty, value); }
		}

		/// <summary>
		/// Sets the property
		/// </summary>
		/// <param name="parameter">Unused.</param>
		protected override void Invoke(object parameter) {

			if (!string.IsNullOrEmpty(this.PropertyName) && (this.Target != null)) {
				Type c = this.Target.GetType();
				PropertyInfo property = c.GetProperty(this.PropertyName);
				if (property == null)
					throw new ArgumentException("Cannot find property " + this.PropertyName + " on object " + this.Target.GetType().Name);

				if (!property.CanWrite)
					throw new ArgumentException("Property is read-only " + this.PropertyName + " on object " + this.Target.GetType().Name);

				object toValue = this.Value;
				TypeConverter typeConverter = ConverterHelper.GetTypeConverter(property.PropertyType);
				Exception innerException = null;
				try {
					if (((typeConverter != null) && (this.Value != null)) && typeConverter.CanConvertFrom(this.Value.GetType()))
						toValue = typeConverter.ConvertFrom(this.Value);

					object fromValue = property.GetValue(this.Target, null);

					if (this.Increment)
						toValue = SetProperty.Add(toValue, fromValue);

					if (this.Duration.HasTimeSpan) {
						Timeline timeline;
						if ((typeof(FrameworkElement).IsAssignableFrom(c) && ((this.PropertyName == "Width") || (this.PropertyName == "Height"))) && double.IsNaN((double)fromValue)) {
							FrameworkElement target = (FrameworkElement)this.Target;
							if (this.PropertyName == "Width")
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
							animation.EasingFunction = this.Ease;
#endif
							timeline = animation;
						}
						else if (typeof(Color).IsAssignableFrom(property.PropertyType)) {
							ColorAnimation animation2 = new ColorAnimation();
							animation2.From = new Color?((Color)fromValue);
							animation2.To = new Color?((Color)toValue);
#if SILVERLIGHT
							animation2.EasingFunction = this.Ease;
#endif
							timeline = animation2;
						}
						else if (typeof(Point).IsAssignableFrom(property.PropertyType)) {
							PointAnimation animation3 = new PointAnimation();
							animation3.From = new Point?((Point)fromValue);
							animation3.To = new Point?((Point)toValue);
#if SILVERLIGHT
							animation3.EasingFunction = this.Ease;
#endif
							timeline = animation3;
						}
						else {
							ObjectAnimationUsingKeyFrames frames = new ObjectAnimationUsingKeyFrames();
							DiscreteObjectKeyFrame frame = new DiscreteObjectKeyFrame();
							frame.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0L));
							frame.Value = fromValue;
							DiscreteObjectKeyFrame frame2 = new DiscreteObjectKeyFrame();
							frame2.KeyTime = KeyTime.FromTimeSpan(this.Duration.TimeSpan);
							frame2.Value = toValue;
							frames.KeyFrames.Add(frame);
							frames.KeyFrames.Add(frame2);
							timeline = frames;
						}
						timeline.Duration = this.Duration;
						storyboard.Children.Add(timeline);
						Storyboard.SetTarget(storyboard, this.Target);
						Storyboard.SetTargetProperty(storyboard, new PropertyPath(property.Name, new object[0]));
						storyboard.Begin();
					}
					else
						property.SetValue(this.Target, toValue, new object[0]);
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
					throw new ArgumentException( innerException.Message);
			
			}
		}

		private static object Add(object a, object b) {
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
	}
}
