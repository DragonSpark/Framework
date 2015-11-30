using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using DragonSpark.Extensions;

// Credit: http://blogs.msdn.com/b/ifeanyie/archive/2010/03/27/9986217.aspx
namespace DragonSpark.Windows.Markup
{
	[MarkupExtensionReturnType( typeof(ImageSource) )]
	public class ImageSourceExtension : DeferredMarkupExtension
	{
		public ImageSourceExtension()
		{}

		public ImageSourceExtension( string sourceUri ) : this( new Uri( sourceUri ) )
		{}

		public ImageSourceExtension( Uri sourceUri ) : this()
		{
			Source = sourceUri;
		}

		public Uri Source { get; set; }

		public ImageSource BusySource { get; set; }

		public ImageSource ErrorSource { get; set; }

		protected override object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			var baseUri = serviceProvider.Get<IUriContext>().With( context => context.BaseUri );
			
			var targetObject = serviceProvider.Get<IProvideValueTarget>().With( target => target.TargetObject as DependencyObject );
			
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
