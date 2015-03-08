using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.View
{
	[ContentProperty( "Effects" )]
	public class CompositeAvailabilityEffect : IAvailabilityEffect
	{
		public ObservableCollection<IAvailabilityEffect> Effects
		{
			get { return effects ?? ( effects = new ObservableCollection<IAvailabilityEffect>() ); }
		}	ObservableCollection<IAvailabilityEffect> effects;

		public void ApplyTo( DependencyObject target, bool isAvailable )
		{
			Effects.Apply( item => item.ApplyTo( target, isAvailable ) );
		}
	}
}