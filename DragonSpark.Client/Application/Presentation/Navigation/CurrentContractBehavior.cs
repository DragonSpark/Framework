using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism;

namespace DragonSpark.Application.Presentation.Navigation
{
    public class CurrentContractBehavior : Behavior<Frame>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Navigated += OnNavigated;
			// AssociatedObject.EnsureLoaded( x => CurrentContract.Null( () => System.Windows.Application.Current.Host.NavigationState.NullIfEmpty().Transform( y => new Uri( y, UriKind.Relative ) ).NotNull( Update ) ) );
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.Navigated -= OnNavigated;
		}

		void OnNavigated( object sender, NavigationEventArgs e )
		{
			Update( e.Uri );
		}

		void Update( Uri current )
		{
			var uri = AssociatedObject.UriMapper.MapUri( current );
			var fullName = uri.ToString();
			CurrentContract = UriParsingHelper.GetQuery( uri ).NullIfEmpty().Transform( x => fullName.Replace( x, string.Empty ) ) ?? fullName;
		}

		public string CurrentContract
		{
			get { return GetValue( CurrentContractProperty ).To<string>(); }
			set { SetValue( CurrentContractProperty, value ); }
		}	public static readonly DependencyProperty CurrentContractProperty = DependencyProperty.Register( "CurrentContract", typeof(string), typeof(CurrentContractBehavior), null );

	}
}