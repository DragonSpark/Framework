using System.Windows;
using System.Windows.Media.Animation;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public static class StoryboardCompensations
	{
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.RegisterAttached( "Target", typeof(DependencyObject), typeof(StoryboardCompensations),
			                                     new PropertyMetadata( OnTargetPropertyChanged ) );

		static void OnTargetPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			o.As<Storyboard>( x => Storyboard.SetTarget( x, e.NewValue.To<DependencyObject>() ) );
		}

		public static DependencyObject GetTarget( Storyboard element )
		{
			return (DependencyObject)element.GetValue( TargetProperty );
		}

		public static void SetTarget( Storyboard element, DependencyObject value )
		{
			element.SetValue( TargetProperty, value );
		}
	}
}