using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	[ContentProperty( "IndexResolver" )]
	public class ItemIndex : Behavior<FrameworkElement>
	{
		IPagedCollectionView PagedCollectionView { get; set; }

		public string Testes
		{
			get { return GetValue( TestesProperty ).To<string>(); }
			set { SetValue( TestesProperty, value ); }
		}	public static readonly DependencyProperty TestesProperty = DependencyProperty.Register( "Testes", typeof(string), typeof(ItemIndex), null );


		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObjectLoaded;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			PagedCollectionView.NotNull( x =>
			{
				PagedCollectionView.PageChanged -= PagedCollectionViewPageChanged;
			} );
			base.OnDetaching();
		}

		void AssociatedObjectLoaded( object sender, RoutedEventArgs e )
		{
			PagedCollectionView = IndexResolver.ResolvePageView( AssociatedObject );
			PagedCollectionView.NotNull( x =>
			{
				PagedCollectionView.PageChanged += PagedCollectionViewPageChanged;
			} );
			AssociatedObject.Loaded -= AssociatedObjectLoaded;

			Refresh();
		}

		void Refresh()
		{
			var total = PagedCollectionView.Transform( z => z.TotalItemCount );
			Top = PagedCollectionView.Transform( z => z.PageIndex * z.PageSize );
			Bottom = Math.Min( total, Top + PagedCollectionView.Transform( y => y.PageSize ) ) - 1;

			Index = IndexResolver.ResolveIndex( AssociatedObject ) + Top;

			Testes = ( Index + 1 ).ToString();

			NextIndex = Index + 1;
			PreviousIndex = Index - 1;

			IndexFromLast = total - Index - 1;
			PreviousIndexFromLast = IndexFromLast + 1;
			NextIndexFromLast = IndexFromLast + 1;

			IsFirst = Index == 0;
			IsLast = ( Index + 1 ) == total;
			IsTop = Index == Top;
			IsBottom = Index == Bottom;
		}

		void PagedCollectionViewPageChanged( object sender, EventArgs e )
		{
			Refresh();
		}

		public IIndexResolver IndexResolver
		{
			get { return GetValue( IndexResolverProperty ).To<IIndexResolver>(); }
			set { SetValue( IndexResolverProperty, value ); }
		}	public static readonly DependencyProperty IndexResolverProperty = DependencyProperty.Register( "IndexResolver", typeof(IIndexResolver), typeof(ItemIndex), new PropertyMetadata( new ItemsControlIndexResolver() ) );
		
		public bool IsFirst
		{
			get { return GetValue( IsFirstProperty ).To<bool>(); }
			private set { SetValue( IsFirstProperty, value ); }
		}	public static readonly DependencyProperty IsFirstProperty = DependencyProperty.Register( "IsFirst", typeof(bool), typeof(ItemIndex), null );

		public bool IsLast
		{
			get { return GetValue( IsLastProperty ).To<bool>(); }
			private set { SetValue( IsLastProperty, value ); }
		}	public static readonly DependencyProperty IsLastProperty = DependencyProperty.Register( "IsLast", typeof(bool), typeof(ItemIndex), null );

		public int Index
		{
			get { return GetValue( IndexProperty ).To<int>(); }
			private set { SetValue( IndexProperty, value ); }
		}	public static readonly DependencyProperty IndexProperty = DependencyProperty.Register( "Index", typeof(int), typeof(ItemIndex), null );

		public int NextIndex
		{
			get { return GetValue( NextIndexProperty ).To<int>(); }
			private set { SetValue( NextIndexProperty, value ); }
		}	public static readonly DependencyProperty NextIndexProperty = DependencyProperty.Register( "NextIndex", typeof(int), typeof(ItemIndex), null );

		public int PreviousIndex
		{
			get { return GetValue( PreviousIndexProperty ).To<int>(); }
			private set { SetValue( PreviousIndexProperty, value ); }
		}	public static readonly DependencyProperty PreviousIndexProperty = DependencyProperty.Register( "PreviousIndex", typeof(int), typeof(ItemIndex), null );

		public int PreviousIndexFromLast
		{
			get { return GetValue( PreviousIndexFromLastProperty ).To<int>(); }
			set { SetValue( PreviousIndexFromLastProperty, value ); }
		}	public static readonly DependencyProperty PreviousIndexFromLastProperty = DependencyProperty.Register( "PreviousIndexFromLast", typeof(int), typeof(ItemIndex), null );
	
		public int IndexFromLast
		{
			get { return GetValue( IndexFromLastProperty ).To<int>(); }
			private set { SetValue( IndexFromLastProperty, value ); }
		}	public static readonly DependencyProperty IndexFromLastProperty = DependencyProperty.Register( "IndexFromLast", typeof(int), typeof(ItemIndex), null );

		public int NextIndexFromLast
		{
			get { return GetValue( NextIndexFromLastProperty ).To<int>(); }
			set { SetValue( NextIndexFromLastProperty, value ); }
		}	public static readonly DependencyProperty NextIndexFromLastProperty = DependencyProperty.Register( "NextIndexFromLast", typeof(int), typeof(ItemIndex), null );

		public int Top
		{
			get { return GetValue( TopProperty ).To<int>(); }
			set { SetValue( TopProperty, value ); }
		}	public static readonly DependencyProperty TopProperty = DependencyProperty.Register( "Top", typeof(int), typeof(ItemIndex), null );

		public int Bottom
		{
			get { return GetValue( BottomProperty ).To<int>(); }
			set { SetValue( BottomProperty, value ); }
		}	public static readonly DependencyProperty BottomProperty = DependencyProperty.Register( "Bottom", typeof(int), typeof(ItemIndex), null );

		public bool IsTop
		{
			get { return GetValue( IsTopProperty ).To<bool>(); }
			set { SetValue( IsTopProperty, value ); }
		}	public static readonly DependencyProperty IsTopProperty = DependencyProperty.Register( "IsTop", typeof(bool), typeof(ItemIndex), null );

		public bool IsBottom
		{
			get { return GetValue( IsBottomProperty ).To<bool>(); }
			set { SetValue( IsBottomProperty, value ); }
		}	public static readonly DependencyProperty IsBottomProperty = DependencyProperty.Register( "IsBottom", typeof(bool), typeof(ItemIndex), null );
	}
}