using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DragonSpark.Extensions;

// Credit: http://blogs.msdn.com/b/ifeanyie/archive/2010/03/27/9986217.aspx
namespace DragonSpark.Application.Markup
{
	[MarkupExtensionReturnType( typeof(ImageSource) )]
	public class AsyncImageSourceExtension : DeferredMarkupExtension
	{
		public AsyncImageSourceExtension()
		{}

		public AsyncImageSourceExtension( Uri sourceUri ) : this()
		{
			Source = sourceUri;
		}

		public Uri Source { get; set; }

		public ImageSource BusySource { get; set; }

		public ImageSource ErrorSource { get; set; }

		protected override object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			var baseUri = serviceProvider.Get<IUriContext>().Transform( context => context.BaseUri );
			
			var targetObject = serviceProvider.Get<IProvideValueTarget>().Transform( target => target.TargetObject as DependencyObject );
			
			var imageUri = ResolveUri( targetObject, baseUri, Source );

			if ( imageUri != null && imageUri.IsAbsoluteUri )
			{
				Task.Run( () => SetFrozenImageSourceFromUri( imageUri, setter ) )/*.ConfigureAwait( false )*/;
			}

			return BusySource;
		}

		object SetFrozenImageSourceFromUri( Uri uri, IMarkupTargetValueSetter setter )
		{
			ImageSourceOperations.GetFrozenImageSourceFromUri( uri, 
				x =>
				{
					setter.SetValue( x );
					setter.Dispose();
				}, 
				e =>
				{
					setter.SetValue( ErrorSource );
					setter.Dispose();
				} );

			return null;
		}

		/// <summary>
		///     Resolve relative Uri's
		/// </summary>
		/// <param name="dataContextSource">If sourceUri is null try to substitute dataContextSource.DataContext</param>
		/// <returns></returns>
		static Uri ResolveUri( DependencyObject dataContextSource, Uri baseUri, Uri sourceUri )
		{
			if ( sourceUri == null && dataContextSource != null )
			{
				sourceUri = dataContextSource.GetValue( FrameworkElement.DataContextProperty ) as Uri;
			}

			var result = baseUri != null && baseUri.IsAbsoluteUri && sourceUri != null ? new Uri( baseUri, sourceUri ) : sourceUri;
			return result;
		}
	}

	public static class ImageSourceOperations
	{
		public static void GetFrozenImageSourceFromUri( Uri imageUri, Action<ImageSource> completedCallback, Action<Exception> failedCallback )
		{
			if ( imageUri == null )
			{
				completedCallback( null );
			}

			BitmapDecoder decoder;
			Exception decodeException;
			try
			{
				decoder = BitmapDecoder.Create( imageUri, BitmapCreateOptions.None, BitmapCacheOption.OnLoad );
				decodeException = null;
			}
			catch ( InvalidOperationException e )
			{
				decoder = null;
				decodeException = e;
			}

			if ( decoder != null && decoder.IsDownloading )
			{
				RegisterDownloadCompleted( decoder, completedCallback, failedCallback );
				RegisterDownloadFailed( decoder, failedCallback );
			}
			else
			{
				if ( decodeException != null )
				{
					failedCallback( decodeException );
				}
				else
				{
					HandleImageAvailbable( decoder, completedCallback, failedCallback );
				}
			}
		}

		static void RegisterDownloadCompleted( BitmapDecoder decoder, Action<ImageSource> callback, Action<Exception> failedCallback )
		{
			EventHandler result = null;

			result = ( sender, e ) =>
				{
					var senderAsDecoder = sender as BitmapDecoder;
					if ( senderAsDecoder != null )
					{
						senderAsDecoder.DownloadCompleted -= result;
						HandleImageAvailbable( senderAsDecoder, callback, failedCallback );
					}
				};

			decoder.DownloadCompleted += result;
		}

		static void RegisterDownloadFailed( BitmapDecoder decoder, Action<Exception> failedCallback )
		{
			EventHandler<ExceptionEventArgs> result = null;

			result = ( sender, e ) =>
				{
					var senderAsDecoder = sender as BitmapDecoder;
					if ( senderAsDecoder != null )
					{
						senderAsDecoder.DownloadFailed -= result;
						failedCallback( e.ErrorException );
					}
				};

			decoder.DownloadFailed += result;
		}

		static void HandleImageAvailbable( BitmapDecoder decoder, Action<ImageSource> completedCallback, Action<Exception> failedCallback )
		{
			if ( decoder != null && decoder.Frames.Count > 0 )
			{
				ImageSource frame = decoder.Frames[0];
				frame.Freeze();
				completedCallback( frame );
			}
			else
			{
				failedCallback( null );
			}
		}
	}

	/*public static class AsyncScheduler
	{
		public static void Post( Action callback )
		{
			ThreadPool.QueueUserWorkItem(x => callback());
		}

		[STAThread]
		static void ThreadStartCallback( object arg )
		{
			var dispatcher = Dispatcher.CurrentDispatcher;
			dispatcher.BeginInvoke( DispatcherPriority.Normal, (Action)arg );
			Dispatcher.Run();
		}
	}*/
}
