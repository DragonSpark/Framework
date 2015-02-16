using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using DragonSpark.Client.Windows.Compensations.Rendering;
using Xamarin.Forms;
using FileAccess = Xamarin.Forms.FileAccess;
using FileMode = Xamarin.Forms.FileMode;
using FileShare = Xamarin.Forms.FileShare;

namespace DragonSpark.Client.Windows.Compensations.Infrastructure
{
	class PlatformServices : IPlatformServices
	{
		public class Timer : ITimer
		{
			readonly System.Threading.Timer timer;

			public Timer( System.Threading.Timer timer )
			{
				this.timer = timer;
			}

			public void Change( int dueTime, int period )
			{
				timer.Change( dueTime, period );
			}

			public void Change( long dueTime, long period )
			{
				timer.Change( dueTime, period );
			}

			public void Change( TimeSpan dueTime, TimeSpan period )
			{
				timer.Change( dueTime, period );
			}

			public void Change( uint dueTime, uint period )
			{
				timer.Change( dueTime, period );
			}
		}

		public class IsolatedStorageFile : IIsolatedStorageFile
		{
			readonly System.IO.IsolatedStorage.IsolatedStorageFile isolatedStorageFile;

			public IsolatedStorageFile( System.IO.IsolatedStorage.IsolatedStorageFile isolatedStorageFile )
			{
				this.isolatedStorageFile = isolatedStorageFile;
			}

			public Task<bool> GetDirectoryExistsAsync( string path )
			{
				return Task.FromResult( isolatedStorageFile.DirectoryExists( path ) );
			}

			public Task CreateDirectoryAsync( string path )
			{
				isolatedStorageFile.CreateDirectory( path );
				return Task.FromResult( true );
			}

			public Task<Stream> OpenFileAsync( string path, FileMode mode, FileAccess access )
			{
				Stream result = isolatedStorageFile.OpenFile( path, (System.IO.FileMode)mode, (System.IO.FileAccess)access );
				return Task.FromResult( result );
			}

			public Task<Stream> OpenFileAsync( string path, FileMode mode, FileAccess access, FileShare share )
			{
				Stream result = isolatedStorageFile.OpenFile( path, (System.IO.FileMode)mode, (System.IO.FileAccess)access, (System.IO.FileShare)share );
				return Task.FromResult( result );
			}

			public Task<bool> GetFileExistsAsync( string path )
			{
				return Task.FromResult( isolatedStorageFile.FileExists( path ) );
			}

			public Task<DateTimeOffset> GetLastWriteTimeAsync( string path )
			{
				return Task.FromResult( isolatedStorageFile.GetLastWriteTime( path ) );
			}
		}

		static readonly MD5CryptoServiceProvider Checksum = new MD5CryptoServiceProvider();

		public bool IsInvokeRequired
		{
			get { return !System.Windows.Application.Current.Dispatcher.CheckAccess(); }
		}

		public double GetNamedSize( NamedSize size, Type targetElementType, bool useOldSizes )
		{
			switch ( size )
			{
				case NamedSize.Default:
					break;
				case NamedSize.Micro:
					return (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"] - 3.0;
				case NamedSize.Small:
					return (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"];
				case NamedSize.Medium:
					if ( !useOldSizes )
					{
						return (double)System.Windows.Application.Current.Resources["PhoneFontSizeMedium"];
					}
					break;
				case NamedSize.Large:
					return (double)System.Windows.Application.Current.Resources["PhoneFontSizeLarge"];
				default:
					throw new ArgumentOutOfRangeException( "size" );
			}
			if ( typeof(Label).IsAssignableFrom( targetElementType ) )
			{
				return (double)System.Windows.Application.Current.Resources["PhoneFontSizeNormal"];
			}
			return (double)System.Windows.Application.Current.Resources["PhoneFontSizeMedium"];
		}

		public void OpenUriAction( Uri uri )
		{
			Process.Start( uri.ToString() );
		}

		public void BeginInvokeOnMainThread( Action action )
		{
			System.Windows.Application.Current.Dispatcher.BeginInvoke( action );
		}

		public void StartTimer( TimeSpan interval, Func<bool> callback )
		{
			var timer = new DispatcherTimer
			{
				Interval = interval
			};
			timer.Start();
			timer.Tick += delegate
			{
				if ( !callback() )
				{
					timer.Stop();
				}
			};
		}

		public Task<Stream> GetStreamAsync( Uri uri, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<Stream>();
			try
			{
				var request = WebRequest.CreateHttp( uri );
				request.AllowReadStreamBuffering = true;
				request.BeginGetResponse( delegate( IAsyncResult ar )
				{
					if ( cancellationToken.IsCancellationRequested )
					{
						tcs.SetCanceled();
						return;
					}
					try
					{
						var responseStream = request.EndGetResponse( ar ).GetResponseStream();
						tcs.TrySetResult( responseStream );
					}
					catch ( Exception exception2 )
					{
						tcs.TrySetException( exception2 );
					}
				}, null );
			}
			catch ( Exception exception )
			{
				tcs.TrySetException( exception );
			}
			return tcs.Task;
		}

		public Assembly[] GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		public ITimer CreateTimer( Action<object> callback )
		{
			return new Timer( new System.Threading.Timer( delegate( object o ) { callback( o ); } ) );
		}

		public ITimer CreateTimer( Action<object> callback, object state, int dueTime, int period )
		{
			return new Timer( new System.Threading.Timer( delegate( object o ) { callback( o ); }, state, dueTime, period ) );
		}

		public ITimer CreateTimer( Action<object> callback, object state, long dueTime, long period )
		{
			return new Timer( new System.Threading.Timer( delegate( object o ) { callback( o ); }, state, dueTime, period ) );
		}

		public ITimer CreateTimer( Action<object> callback, object state, TimeSpan dueTime, TimeSpan period )
		{
			return new Timer( new System.Threading.Timer( delegate( object o ) { callback( o ); }, state, dueTime, period ) );
		}

		public ITimer CreateTimer( Action<object> callback, object state, uint dueTime, uint period )
		{
			return new Timer( new System.Threading.Timer( delegate( object o ) { callback( o ); }, state, dueTime, period ) );
		}

		public IIsolatedStorageFile GetUserStoreForApplication()
		{
			return new IsolatedStorageFile( System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication() );
		}

		public string GetMD5Hash( string input )
		{
			var array = Checksum.ComputeHash( Encoding.UTF8.GetBytes( input ) );
			var array2 = new char[32];
			for ( var i = 0; i < 16; i++ )
			{
				array2[i * 2] = (char)hex( array[i] >> 4 );
				array2[i * 2 + 1] = (char)hex( array[i] & 15 );
			}
			return new string( array2 );
		}

		static int hex( int v )
		{
			if ( v < 10 )
			{
				return 48 + v;
			}
			return 97 + v - 10;
		}
	}
}
