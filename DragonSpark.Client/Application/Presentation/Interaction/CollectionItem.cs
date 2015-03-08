using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class CollectionItem : Behavior<FrameworkElement>
	{
		public int Index
		{
			get { return GetValue( IndexProperty ).To<int>(); }
			set { SetValue( IndexProperty, value ); }
		}	public static readonly DependencyProperty IndexProperty = DependencyProperty.Register( "Index", typeof(int), typeof(CollectionItem), new PropertyMetadata( Check ) );

		public IEnumerable Source
		{
			get { return GetValue( SourceProperty ).To<IEnumerable>(); }
			set { SetValue( SourceProperty, value ); }
		}	public static readonly DependencyProperty SourceProperty = DependencyProperty.Register( "Source", typeof(IEnumerable), typeof(CollectionItem), new PropertyMetadata( Check ) );

		public object Value
		{
			get { return GetValue( ValueProperty ).To<object>(); }
			set { SetValue( ValueProperty, value ); }
		}	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof(object), typeof(CollectionItem), null );

		static void Check( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<CollectionItem>( x => x.Value = x.Source.Transform( y => y.Cast<object>().ElementAtOrDefault( x.Index ) ) );
		}
	}
}