using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Client.Windows.Compensations.Interaction
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
