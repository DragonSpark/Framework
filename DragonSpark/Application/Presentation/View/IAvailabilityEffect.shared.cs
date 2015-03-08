using System.ComponentModel;
using System.Windows;

namespace DragonSpark.Application.Presentation.View
{
	[TypeConverter(typeof(AvailabilityEffectConverter))]
	public interface IAvailabilityEffect
	{
		/// <summary>
		/// Applies the effect to the target.
		/// </summary>
		/// <param name="target">The element.</param>
		/// <param name="isAvailable">Determines how the effect will be applied to the target.</param>
		void ApplyTo(DependencyObject target, bool isAvailable);
	}
}