using System;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static class ApplicationExtensions
	{
		public static UIElement GetShell( this System.Windows.Application target )
		{
			var result = target.GetShell<UIElement>();
			return result;
		}

		public static Uri GetUri( this System.Windows.Application target )
		{
			var result = target.Host.Source;
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "target")]
		public static Uri GetHostUri( this System.Windows.Application target )
		{
			var result = HtmlPage.Document.DocumentUri;
			return result;
		}

		public static TShell GetShell<TShell>( this System.Windows.Application target ) where TShell : UIElement
		{
			var result = target.RootVisual.As<TShell>();
			return result;
		}

		public static TService GetService<TService>( this System.Windows.Application target )
		{

			var query = from service in target.ApplicationLifetimeObjects.Cast<object>()
			            where typeof(TService).IsAssignableFrom( service.GetType() )
			            select service;
			var result = query.OfType<TService>().FirstOrDefault();
			return result;
		}
	}
}