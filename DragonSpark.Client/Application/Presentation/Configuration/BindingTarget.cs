using System.ComponentModel;
using System.Windows;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
	class BindingTarget : FrameworkElement, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate {};

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register( "Value", typeof(object), typeof(BindingTarget), new PropertyMetadata( null, OnValueChanged ) );

		public object Value
		{
			get { return GetValue( ValueProperty ); }
			set { SetValue( ValueProperty, value ); }
		}

		static void OnValueChanged( DependencyObject depObj, DependencyPropertyChangedEventArgs e )
		{
			depObj.As<BindingTarget>( x => x.OnPropertyChanged( "Value" ) );
		}

		protected void OnPropertyChanged( string name )
		{
			PropertyChanged( this, new PropertyChangedEventArgs( name ) );
		}
	}
}