using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DragonSpark.Windows.Markup
{
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
}