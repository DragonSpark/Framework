using System.Windows;

namespace DragonSpark.Application.Presentation.View
{
	/// <summary>
	/// DragonSpark implementations of <see cref="IAvailabilityEffect"/>.
	/// </summary>
	public static class AvailabilityEffect
	{
		/// <summary>
		/// The element is not affected by changes in availability.
		/// </summary>
		public static IAvailabilityEffect None { get { return NoneField; } }
		static readonly IAvailabilityEffect NoneField = new NoneEffect();

		/// <summary>
		/// The element is not affected by changes in availability.
		/// </summary>
		private class NoneEffect : IAvailabilityEffect
		{
			/// <summary>
			/// Applies the effect to the target.
			/// </summary>
			/// <param name="target">The element.</param>
			/// <param name="isAvailable">Determines how the effect will be applied to the target.</param>
			public void ApplyTo(DependencyObject target, bool isAvailable) { }
		}

		/// <summary>
		/// This effect can disable the UI.
		/// </summary>
		public static IAvailabilityEffect Disable { get { return DisableField; } }
		static readonly IAvailabilityEffect DisableField = new DisableEffect();

		/// <summary>
		/// This effect can disable the UI.
		/// </summary>
		private class DisableEffect : IAvailabilityEffect
		{
			/// <summary>
			/// Applies the effect to the target.
			/// </summary>
			/// <param name="target">The element.</param>
			/// <param name="isAvailable">Determines how the effect will be applied to the target.</param>
			public void ApplyTo(DependencyObject target, bool isAvailable)
			{
#if SILVERLIGHT
				if(target == null)
					return;

				var propertyInfo = target.GetType().GetProperty("IsEnabled");

				if(propertyInfo == null)
					return;

				var value = (bool)propertyInfo.GetValue(target, null);
				
				if (value && !isAvailable) propertyInfo.SetValue(target, false, null);
				else if (!value && isAvailable) propertyInfo.SetValue(target, true, null);
#else
				var element = target as UIElement;
				if (element != null)
				{
					if (element.IsEnabled && !isAvailable)
						element.IsEnabled = false;
					else if (!element.IsEnabled && isAvailable)
						element.IsEnabled = true;
				}
				else
				{
					var ce = target as ContentElement;
					if (ce != null)
					{
						if (ce.IsEnabled && !isAvailable)
							ce.IsEnabled = false;
						else if (!ce.IsEnabled && isAvailable)
							ce.IsEnabled = true;
					}
				}
#endif
			}
		}

		/// <summary>
		/// This effect can hide the UI.
		/// </summary>
		public static IAvailabilityEffect Hide { get { return HideField; } }
		static readonly IAvailabilityEffect HideField = new HideEffect();

		/// <summary>
		/// This effect can hide the UI.
		/// </summary>
		private class HideEffect : IAvailabilityEffect
		{
#if SILVERLIGHT
			public static readonly DependencyProperty OldOpacityProperty =
				DependencyProperty.RegisterAttached(
					"OldOpacity",
					typeof(double),
					typeof(HideEffect),
					null
				);
#endif

			/// <summary>
			/// Applies the effect to the target.
			/// </summary>
			/// <param name="target">The element.</param>
			/// <param name="isAvailable">Determines how the effect will be applied to the target.</param>
			public void ApplyTo(DependencyObject target, bool isAvailable)
			{
#if SILVERLIGHT
				var element = target as UIElement;
				if(element == null) 
					return;

				if (element.Opacity != 0 && !isAvailable) 
				{
					element.SetValue(OldOpacityProperty, element.Opacity);
					element.Opacity = 0;
				}
				else if (element.Opacity == 0 && isAvailable) 
				{
					object oldValue = element.GetValue(OldOpacityProperty);
					double opacity = oldValue == null ? 1 : (double)oldValue;
					element.Opacity = opacity;
				}
#else
				var element = target as UIElement;
				if (element == null)
					return;

				if (element.Visibility == Visibility.Visible && !isAvailable)
					element.Visibility = Visibility.Hidden;
				else if (element.Visibility != Visibility.Visible && isAvailable)
					element.Visibility = Visibility.Visible;
#endif
			}
		}

		/// <summary>
		/// This effect can collapse the UI.
		/// </summary>
		public static IAvailabilityEffect Collapse { get { return CollapseField; } }
		static readonly IAvailabilityEffect CollapseField = new CollapseEffect();

		/// <summary>
		/// This effect can collapse the UI.
		/// </summary>
		private class CollapseEffect : IAvailabilityEffect
		{
			/// <summary>
			/// Applies the effect to the target.
			/// </summary>
			/// <param name="target">The element.</param>
			/// <param name="isAvailable">Determines how the effect will be applied to the target.</param>
			public void ApplyTo(DependencyObject target, bool isAvailable)
			{
				var element = target as UIElement;
				if (element == null)
					return;

				if (element.Visibility == Visibility.Visible && !isAvailable) element.Visibility = Visibility.Collapsed;
				if (element.Visibility != Visibility.Visible && isAvailable) element.Visibility = Visibility.Visible;
			}
		}

		 /// <summary>
		/// A property representing the availability effect of a given message.
		/// </summary>
		public static readonly DependencyProperty AvailabilityEffectProperty =
			DependencyProperty.RegisterAttached(
				"AvailabilityEffect",
				typeof(IAvailabilityEffect),
				typeof(AvailabilityEffect),
				null
				);

		/// <summary>
		/// Sets the availability effect.
		/// </summary>
		/// <param name="dependencyObject">The d.</param>
		/// <param name="effect">The effect.</param>
		public static void SetAvailabilityEffect(DependencyObject dependencyObject, IAvailabilityEffect effect)
		{
			dependencyObject.SetValue(AvailabilityEffectProperty, effect);
		}

		/// <summary>
		/// Gets the availability effect.
		/// </summary>
		/// <param name="dependencyObject">The d.</param>
		/// <returns></returns>
		public static IAvailabilityEffect GetAvailabilityEffect(DependencyObject dependencyObject)
		{
			return dependencyObject.GetValue(AvailabilityEffectProperty) as IAvailabilityEffect;
		}
	}
}