using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Application.Client.Forms.ComponentModel;
using Xamarin.Forms;
using Binding = System.Windows.Data.Binding;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class MasterDetailRenderer : VisualElementRenderer<MasterDetailPage, FrameworkElement>
	{
		readonly Border popup = new Border();

		UIElement masterElement, detailElement;
		
		/*readonly SlideTransition inTransition = new SlideTransition
		{
			Mode = SlideTransitionMode.SlideUpFadeIn
		};

		readonly SlideTransition outTransition = new SlideTransition
		{
			Mode = SlideTransitionMode.SlideDownFadeOut
		};*/

		// ITransition toggleTransition;
		public MasterDetailRenderer()
		{
			AutoPackage = false;

			SetBinding( ShellProperties.TitleProperty, new Binding( "Element.Title" ) );
		}

		public bool Visible { get; private set; }

		protected override void OnElementChanged( ElementChangedEventArgs<MasterDetailPage> e )
		{
			base.OnElementChanged( e );
			if ( e.OldElement != null )
			{
				e.OldElement.BackButtonPressed -= HandleBackButtonPressed;
			}
			if ( e.NewElement != null )
			{
				e.NewElement.BackButtonPressed += HandleBackButtonPressed;
			}
			LoadDetail();
			LoadMaster();
			UpdateSizes();
			Loaded += delegate
			{
				if ( base.Element.IsPresented )
				{
					this.Toggle();
				}
				base.Element.SendAppearing();
			};
			Unloaded += delegate { base.Element.SendDisappearing(); };
		}

		void HandleBackButtonPressed( object sender, BackButtonPressedEventArgs e )
		{
			if ( Visible )
			{
				Toggle();
				e.Handled = true;
			}
		}

		protected override void UpdateNativeWidget()
		{
			base.UpdateNativeWidget();
			UpdateSizes();
		}

		internal void Toggle()
		{
			// TODO: Uncomment
			/*var platform = Element.ApplicationHost as ApplicationHost;
			var container = platform.GetVisualElement();
			/*if ( toggleTransition == null )
			{}#1#
			if ( Visible )
			{
				container.Children.Remove( popup );
				/*toggleTransition = outTransition.GetTransition( popup );
				toggleTransition.Begin();
				toggleTransition.Completed += ( sender, args ) =>
				{
					toggleTransition.Stop();
					container.Children.Remove( this.popup );
					toggleTransition = null;
				};#1#
			}
			else
			{
				popup.Child = masterElement;
				container.Children.Add( popup );
				/*toggleTransition = inTransition.GetTransition( popup );
				toggleTransition.Begin();
				toggleTransition.Completed += delegate
				{
					toggleTransition.Stop();
					toggleTransition = null;
				};#1#
			}
			Visible = !Visible;
			( (IElementController)Element ).SetValueFromRenderer( MasterDetailPage.IsPresentedProperty, Visible );*/
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			if ( e.PropertyName != "Detail" )
			{
				switch ( e.PropertyName )
				{
					case "Master":
						LoadMaster();
						UpdateSizes();
						return;
				}
				if ( e.PropertyName == MasterDetailPage.IsPresentedProperty.PropertyName && Visible != Element.IsPresented )
				{
					Toggle();
				}
			}
			else
			{
				LoadDetail();
				UpdateSizes();
			}
		}

		void LoadMaster()
		{
			var master = Element.Master;
			if ( master.GetRenderer() == null )
			{
				master.SetRenderer( RendererFactory.GetRenderer( master ) );
			}
			masterElement = master.GetRenderer() as UIElement;
			var panel = masterElement as Panel;
			if ( panel != null && master.BackgroundColor == Color.Default )
			{
				panel.Background = Color.Black.ToBrush();
			}
		}

		void LoadDetail()
		{
			var detail = Element.Detail;
			if ( detail.GetRenderer() == null )
			{
				detail.SetRenderer( RendererFactory.GetRenderer( detail ) );
			}
			detailElement = ( detail.GetRenderer() as UIElement );
			Children.Clear();
			if ( detailElement != null )
			{
				Children.Add( detailElement );
			}
		}

		void UpdateSizes()
		{
			if ( double.IsNaN( Width ) || double.IsNaN( Height ) )
			{
				return;
			}
			/*var platform = Element.ApplicationHost as ApplicationHost;
			var size = platform.Size;
			Element.MasterBounds = new Rectangle( 0.0, 0.0, size.Width - 20.0, size.Height - 20.0 );
			Element.DetailBounds = new Rectangle( 0.0, 0.0, Width, Height );*/
			popup.Width = Width - 20.0;
			popup.Height = Height - 20.0;
			SetLeft( popup, 10.0 );
			SetTop( popup, 10.0 );
		}
	}
}
