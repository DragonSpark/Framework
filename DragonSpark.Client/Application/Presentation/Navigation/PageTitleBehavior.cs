using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class PageTitleBehavior : Behavior<Page>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
		}

		void AssociatedObjectOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			var pages = AssociatedObject.GetSelfAndAncestors().OfType<Frame>().Select( x => x.Content.As<Page>() ).Reverse().ToArray();
			pages.Apply( x =>
			{
				var current = ApplicationTitle.GetTitle( x );
				current.Null( () => ApplicationTitle.SetTitle( x, x.Title ?? string.Empty ) );
			});

			var titles = new[] { System.Windows.Application.Current.RootVisual }.Concat( pages ).Cast<FrameworkElement>().Select( ApplicationTitle.GetTitle ).Select( x => x.NullIfEmpty() ).NotNull().ToArray();
			var title = string.Join( Separator, titles );
			pages.Apply( x => x.Title = title );
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;

			base.OnDetaching();
		}

		public string Separator
		{
			get { return GetValue( SeparatorProperty ).To<string>(); }
			set { SetValue( SeparatorProperty, value ); }
		}	public static readonly DependencyProperty SeparatorProperty = DependencyProperty.Register( "Separator", typeof(string), typeof(PageTitleBehavior), new PropertyMetadata( " > " ) );
	}
}