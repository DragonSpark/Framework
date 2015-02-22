using System;
using System.Windows;
using System.Windows.Input;
using DragonSpark.Extensions;
using Xamarin.Forms;
using Point = System.Windows.Point;
using Size = Xamarin.Forms.Size;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class TableViewRenderer : ViewRenderer<global::Xamarin.Forms.TableView, TableView>
	{
		TableView view;

		protected override void OnElementChanged( ElementChangedEventArgs<global::Xamarin.Forms.TableView> e )
		{
			base.OnElementChanged( e );
			e.OldElement.With( x =>
			{
				x.ModelChanged -= OnModelChanged;
			} );

			Element.ModelChanged += OnModelChanged;
			view = new TableView
			{
				DataContext = Element.Root
			};
			view.MouseLeftButtonUp += OnTapTable;
			view.MouseRightButtonUp += OnLongPressTable;

			SetNativeControl( view );
		}

		public override SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		{
			var desiredSize = base.GetDesiredSize( widthConstraint, heightConstraint );
			desiredSize.Minimum = new Size( 40.0, 40.0 );
			return desiredSize;
		}

		void OnModelChanged( object sender, EventArgs eventArgs )
		{
			view.DataContext = Element.Root;
		}

		void OnLongPressTable( object sender, MouseEventArgs e )
		{
			int section;
			int row;
			if ( !FindIndices( e, out section, out row ) )
			{
				return;
			}
			Element.Model.RowLongPressed( section, row );
		}

		void OnTapTable( object sender, MouseEventArgs e )
		{
			int section;
			int row;
			if ( !FindIndices( e, out section, out row ) )
			{
				return;
			}
			Element.Model.RowSelected( section, row );
		}

		bool FindIndices( MouseEventArgs e, out int sectionIndex, out int cellIndex )
		{
			sectionIndex = 0;
			cellIndex = 0;
			TableSection tableSection = null;
			Cell cell = null;
			var window = System.Windows.Application.Current.MainWindow;
			var position = e.GetPosition( window );
			if ( Device.Info.CurrentOrientation.IsLandscape() )
			{
				var y = position.Y;
				var y2 = window.RenderSize.Width - position.X /*+ ( SystemTray.IsVisible ? 72 : 0 )*/;
				position = new Point( y, y2 );
			}
			foreach ( var current in window.FindElementsAt<FrameworkElement>( position ) )
			{
				if ( cell == null )
				{
					cell = ( current.DataContext as Cell );
				}
				else
				{
					if ( tableSection != null )
					{
						break;
					}
					tableSection = ( current.DataContext as TableSection );
				}
			}
			if ( cell != null && tableSection != null )
			{
				sectionIndex = Element.Root.IndexOf( tableSection );
				cellIndex = tableSection.IndexOf( cell );
				return true;
			}
			return false;
		}
	}
}
