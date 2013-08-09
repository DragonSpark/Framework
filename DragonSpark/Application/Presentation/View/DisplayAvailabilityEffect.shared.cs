using System.Windows;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.View
{
	public class DisplayAvailabilityEffect : IAvailabilityEffect
	{
		readonly IAvailabilityEffect effect = new AvailabilityEffectConverter().ConvertFrom( "Collapse" ).As<IAvailabilityEffect>();
		public void ApplyTo( DependencyObject target, bool isAvailable )
		{
			effect.ApplyTo( target, !isAvailable );
		}
	}
}