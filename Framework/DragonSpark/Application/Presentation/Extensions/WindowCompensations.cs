using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static class WindowCompensations
	{
		#region CanMaximize
		public static readonly DependencyProperty CanMaximize =
			DependencyProperty.RegisterAttached( "CanMaximize", typeof(bool), typeof(Window),
			                                     new PropertyMetadata( true, OnCanMaximizeChanged ) );

		static void OnCanMaximizeChanged( DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e )
		{
			var window = dependencyObject as Window;
			if ( window != null )
			{
				RoutedEventHandler loadedHandler = null;
				loadedHandler = delegate
				                	{
				                		if ( (bool)e.NewValue )
				                		{
				                			NativeMethods.EnableMaximize( window );
				                		}
				                		else
				                		{
				                			NativeMethods.DisableMaximize( window );
				                		}
				                		window.Loaded -= loadedHandler;
				                	};

				if ( !window.IsLoaded )
				{
					window.Loaded += loadedHandler;
				}
				else
				{
					loadedHandler( null, null );
				}
			}
		}

		public static void SetCanMaximize( DependencyObject dependencyObject, bool value )
		{
			Contract.Requires( dependencyObject != null );

			dependencyObject.SetValue( CanMaximize, value );
		}

		public static bool GetCanMaximize( DependencyObject dependencyObject )
		{
			return (bool)dependencyObject.GetValue( CanMaximize );
		}
		#endregion CanMaximize

		#region CanMinimize
		public static readonly DependencyProperty CanMinimize =
			DependencyProperty.RegisterAttached( "CanMinimize", typeof(bool), typeof(Window),
			                                     new PropertyMetadata( true, OnCanMinimizeChanged ) );

		static void OnCanMinimizeChanged( DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e )
		{
			var window = dependencyObject as Window;
			if ( window != null )
			{
				RoutedEventHandler loadedHandler = null;
				loadedHandler = delegate
				                	{
				                		if ( (bool)e.NewValue )
				                		{
				                			NativeMethods.EnableMinimize( window );
				                		}
				                		else
				                		{
				                			NativeMethods.DisableMinimize( window );
				                		}
				                		window.Loaded -= loadedHandler;
				                	};

				if ( !window.IsLoaded )
				{
					window.Loaded += loadedHandler;
				}
				else
				{
					loadedHandler( null, null );
				}
			}
		}

		public static void SetCanMinimize( DependencyObject dependencyObject, bool value )
		{
			dependencyObject.SetValue( CanMinimize, value );
		}

		public static bool GetCanMinimize( DependencyObject dependencyObject )
		{
			return (bool)dependencyObject.GetValue( CanMinimize );
		}
		#endregion CanMinimize

		#region WindowHelper Nested Class
		static class NativeMethods
		{
			const Int32 GWL_STYLE = -16;
			const Int32 WS_MAXIMIZEBOX = 0x00010000;
			const Int32 WS_MINIMIZEBOX = 0x00020000;

			[DllImport( "User32.dll", EntryPoint = "GetWindowLong" )]
			static extern Int32 GetWindowLongPtr( IntPtr hWnd, Int32 nIndex );

			[DllImport( "User32.dll", EntryPoint = "SetWindowLong" )]
			static extern Int32 SetWindowLongPtr( IntPtr hWnd, Int32 nIndex, Int32 dwNewLong );

			/// <summary>
			/// Disables the maximize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "DragonSpark.Application.Presentation.Extensions.WindowCompensations+NativeMethods.SetWindowLongPtr(System.IntPtr,System.Int32,System.Int32)")]
			public static void DisableMaximize( Window window )
			{
				lock ( window )
				{
					var hWnd = new WindowInteropHelper( window ).Handle;
					var windowStyle = GetWindowLongPtr( hWnd, GWL_STYLE );
					SetWindowLongPtr( hWnd, GWL_STYLE, windowStyle & ~WS_MAXIMIZEBOX );
				}
			}

			/// <summary>
			/// Disables the minimize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "DragonSpark.Application.Presentation.Extensions.WindowCompensations+NativeMethods.SetWindowLongPtr(System.IntPtr,System.Int32,System.Int32)")]
			public static void DisableMinimize( Window window )
			{
				lock ( window )
				{
					var hWnd = new WindowInteropHelper( window ).Handle;
					var windowStyle = GetWindowLongPtr( hWnd, GWL_STYLE );
					SetWindowLongPtr( hWnd, GWL_STYLE, windowStyle & ~WS_MINIMIZEBOX );
				}
			}

			/// <summary>
			/// Enables the maximize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "DragonSpark.Application.Presentation.Extensions.WindowCompensations+NativeMethods.SetWindowLongPtr(System.IntPtr,System.Int32,System.Int32)")]
			public static void EnableMaximize( Window window )
			{
				lock ( window )
				{
					var hWnd = new WindowInteropHelper( window ).Handle;
					var windowStyle = GetWindowLongPtr( hWnd, GWL_STYLE );
					SetWindowLongPtr( hWnd, GWL_STYLE, windowStyle | WS_MAXIMIZEBOX );
				}
			}

			/// <summary>
			/// Enables the minimize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "DragonSpark.Application.Presentation.Extensions.WindowCompensations+NativeMethods.SetWindowLongPtr(System.IntPtr,System.Int32,System.Int32)")]
			public static void EnableMinimize( Window window )
			{
				lock ( window )
				{
					var hWnd = new WindowInteropHelper( window ).Handle;
					var windowStyle = GetWindowLongPtr( hWnd, GWL_STYLE );
					SetWindowLongPtr( hWnd, GWL_STYLE, windowStyle | WS_MINIMIZEBOX );
				}
			}
		}
		#endregion WindowHelper Nested Class
	}
}