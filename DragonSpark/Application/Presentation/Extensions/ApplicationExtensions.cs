using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Navigation;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static class ApplicationExtensions
	{
		public static UIElement GetShell( this System.Windows.Application target )
		{
			Contract.Requires( target != null );

			var result = target.GetShell<UIElement>();
			return result;
		}

		public static Uri GetUri( this System.Windows.Application target )
		{
			Contract.Requires( target != null );
			var result = target.MainWindow.As<NavigationWindow>().Transform( item => item.Source );
			return result;
		}

		public static Uri GetHostUri( this System.Windows.Application target )
		{
			Contract.Requires( target != null );
			var result = target.GetUri();
			return result;
		}

		public static TShell GetShell<TShell>( this System.Windows.Application target ) where TShell : UIElement
		{
			Contract.Requires( target != null );

			var result = target.MainWindow.As<TShell>();
			return result;
		}

		public static TService GetService<TService>( this System.Windows.Application target )
		{
			Contract.Requires( target != null );
			Contract.Requires( target.Resources != null );

			var result = target.Resources.Values.FirstOrDefaultOfType<TService>();
			return result;
		}
	}
}