using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Navigation;
using DragonSpark.Extensions;
using Xamarin.Forms;
using NavigationEventArgs = System.Windows.Navigation.NavigationEventArgs;

namespace DragonSpark.Application.Forms.Rendering
{
	public class WebViewRenderer : ViewRenderer<WebView, WebBrowser>, IWebViewRenderer
	{
		WebNavigationEvent eventState;
		bool updating;

		protected override void OnElementChanged( ElementChangedEventArgs<WebView> e )
		{
			base.OnElementChanged( e );
			if ( Control == null )
			{
				var webBrowser = new WebBrowser();
				webBrowser.Navigated += WebBrowserOnNavigated;
				webBrowser.Navigating += WebBrowserOnNavigating;
				// webBrowser.NavigationFailed += WebBrowserOnNavigationFailed;
				SetNativeControl( webBrowser );
			}
			if ( e.OldElement != null )
			{
				e.OldElement.EvalRequested -= OnEvalRequested;
				e.OldElement.GoBackRequested -= OnGoBackRequested;
				e.OldElement.GoForwardRequested -= OnGoForwardRequested;
				Control.DataContext = null;
			}
			if ( e.NewElement != null )
			{
				e.NewElement.EvalRequested += OnEvalRequested;
				e.NewElement.GoBackRequested += OnGoBackRequested;
				e.NewElement.GoForwardRequested += OnGoForwardRequested;
				Control.DataContext = e.NewElement;
			}
			Load();
		}

		void OnGoForwardRequested( object sender, EventArgs eventArgs )
		{
			if ( Control.CanGoForward )
			{
				eventState = WebNavigationEvent.Forward;
				Control.GoForward();
			}
			UpdateCanGoBackForward();
		}

		void OnGoBackRequested( object sender, EventArgs eventArgs )
		{
			if ( Control.CanGoBack )
			{
				eventState = WebNavigationEvent.Back;
				Control.GoBack();
			}
			UpdateCanGoBackForward();
		}

		void UpdateCanGoBackForward()
		{
			Element.CanGoBack = Control.CanGoBack;
			Element.CanGoForward = Control.CanGoForward;
		}

		void WebBrowserOnNavigating( object sender, NavigatingCancelEventArgs navigatingCancelEventArgs )
		{
			string url = navigatingCancelEventArgs.Uri.IsAbsoluteUri ? navigatingCancelEventArgs.Uri.AbsoluteUri : navigatingCancelEventArgs.Uri.OriginalString;
			var webNavigatingEventArgs = new WebNavigatingEventArgs( eventState, new UrlWebViewSource
			{
				Url = url
			}, url );
			Element.SendNavigating( webNavigatingEventArgs );
			navigatingCancelEventArgs.Cancel = webNavigatingEventArgs.Cancel;
			if ( webNavigatingEventArgs.Cancel )
			{
				eventState = WebNavigationEvent.NewPage;
			}
		}

		void WebBrowserOnNavigationFailed( object sender, NavigationFailedEventArgs navigationFailedEventArgs )
		{
			var url = navigationFailedEventArgs.Uri.IsAbsoluteUri ? navigationFailedEventArgs.Uri.AbsoluteUri : navigationFailedEventArgs.Uri.OriginalString;
			SendNavigated( new UrlWebViewSource
			{
				Url = url
			}, eventState, WebNavigationResult.Failure );
		}

		void WebBrowserOnNavigated( object sender, NavigationEventArgs navigationEventArgs )
		{
			var result = navigationEventArgs.WebResponse.AsTo<HttpWebResponse, bool>( x => new[] { HttpStatusCode.NotFound, HttpStatusCode.InternalServerError }.Contains( x.StatusCode ) ) ? WebNavigationResult.Failure : WebNavigationResult.Success;
			var url = navigationEventArgs.Uri.IsAbsoluteUri ? navigationEventArgs.Uri.AbsoluteUri : navigationEventArgs.Uri.OriginalString;
			SendNavigated( new UrlWebViewSource
			{
				Url = url
			}, eventState, result );

			switch ( result )
			{
				case WebNavigationResult.Success:
					UpdateCanGoBackForward();
					break;
			}
		}

		void SendNavigated( UrlWebViewSource source, WebNavigationEvent evnt, WebNavigationResult result )
		{
			updating = true;
			( (IElementController)Element ).SetValueFromRenderer( WebView.SourceProperty, source );
			updating = false;
			Element.SendNavigated( new WebNavigatedEventArgs( evnt, source, source.Url, result ) );
			UpdateCanGoBackForward();
			eventState = WebNavigationEvent.NewPage;
		}

		void OnEvalRequested( object sender, EventArg<string> eventArg )
		{
			Control.Dispatcher.BeginInvoke( new Action( delegate { this.Control.InvokeScript( "eval", eventArg.Data ); } ) );
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			string propertyName;
			if ( ( propertyName = e.PropertyName ) != null )
			{
				if ( !( propertyName == "Source" ) )
				{
					return;
				}
				if ( !updating )
				{
					Load();
				}
			}
		}

		void Load()
		{
			if ( Element.Source != null )
			{
				Element.Source.Load( this );
			}
			UpdateCanGoBackForward();
		}

		public void LoadUrl( string url )
		{
			Control.Source = new Uri( url );
		}

		public void LoadHtml( string html, string baseUrl )
		{
			Control.NavigateToString( html );
		}
	}
}
