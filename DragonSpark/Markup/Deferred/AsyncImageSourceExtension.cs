using DragonSpark.Extensions;
//-----------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

// Credit: http://blogs.msdn.com/b/ifeanyie/archive/2010/03/27/9986217.aspx
namespace DragonSpark.Application.Markup.Deferred
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

		/// <summary>
		///     Uri of image to download and decode
		/// </summary>
		public Uri Source { get; set; }

		/// <summary>
		///     Image source to display while the primary image is being downloaded and decoded
		/// </summary>
		public ImageSource BusySource { get; set; }

		/// <summary>
		///     Image source to display if there is a download or decode error
		/// </summary>
		public ImageSource ErrorSource { get; set; }

		protected override object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			Uri baseUri = null;
			var uriContextService = serviceProvider.Get<IUriContext>();
			if ( uriContextService != null )
			{
				baseUri = uriContextService.BaseUri;
			}

			DependencyObject targetObject = null;
			var provideValueTarget = serviceProvider.Get<IProvideValueTarget>();
			if ( provideValueTarget != null )
			{
				targetObject = provideValueTarget.TargetObject as DependencyObject;
			}

			var imageUri = ResolveUri( targetObject, baseUri, Source );

			if ( imageUri != null && imageUri.IsAbsoluteUri )
			{
				Task.Run( () => SetFrozenImageSourceFromUri( imageUri, setter ) );
			}

			return BusySource;
		}

		object SetFrozenImageSourceFromUri( Uri uri, IMarkupTargetValueSetter setter )
		{
			Action<ImageSource> setValueAndDispose = x =>
				{
					setter.SetValue( x );
					setter.Dispose();
				};

			var errorSource = ErrorSource;

			Action<Exception> setErrorAndDispose = e =>
				{
					setter.SetValue( errorSource );
					setter.Dispose();
				};

			ImageSourceOperations.GetFrozenImageSourceFromUri( uri, setValueAndDispose, setErrorAndDispose );

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

	public static class AsyncScheduler
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
	}
}
