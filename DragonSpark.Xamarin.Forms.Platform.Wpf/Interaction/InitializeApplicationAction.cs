using DragonSpark.Extensions;
using DragonSpark.Xamarin.Forms.Platform.Wpf.Application;
using System.Windows;
using System.Windows.Interactivity;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Interaction
{
	class InitializeApplicationAction : TriggerAction<FrameworkElement>
	{
		protected override void Invoke( object parameter )
		{
			// var host = new ApplicationHost( Application );

		}
		public global::Xamarin.Forms.Application Application
		{
			get { return GetValue( ApplicationProperty ).To<global::Xamarin.Forms.Application>(); }
			set { SetValue( ApplicationProperty, value ); }
		}	public static readonly DependencyProperty ApplicationProperty = DependencyProperty.Register( "Application", typeof(global::Xamarin.Forms.Application), typeof(InitializeApplicationAction), null );
	}
}
