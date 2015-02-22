using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xamarin.Forms;

namespace DragonSpark.Application.Forms.Rendering
{
	public abstract class VisualElementTracker : IDisposable
	{
		public event EventHandler Updated;
		public abstract FrameworkElement Child { get; set; }
		public abstract void Dispose();

		protected void OnUpdated()
		{
			if ( Updated != null )
			{
				Updated( this, EventArgs.Empty );
			}
		}
	}

	public class VisualElementTracker<TModel, TElement> : VisualElementTracker where TModel : VisualElement where TElement : FrameworkElement
	{
		TElement element;
		FrameworkElement child;
		TModel model;
		bool disposed;

		public TElement Element
		{
			get { return element; }
			set
			{
				if ( element != value )
				{
					if ( element != null )
					{
						element.MouseLeftButtonUp -= ElementOnTap;
						var control = element as Control;
						if ( control != null )
						{
							control.MouseDoubleClick -= ElementOnDoubleTap;
						}
					}
					element = value;
					if ( element != null )
					{
						element.MouseLeftButtonUp += ElementOnTap;
						var control = element as Control;
						if ( control != null )
						{
							control.MouseDoubleClick += ElementOnDoubleTap;
						}
					}
					UpdateNativeControl();
				}
			}
		}

		public TModel Model
		{
			get { return model; }
			set
			{
				if ( model == value )
				{
					return;
				}
				if ( model != null )
				{
					model.BatchCommitted -= HandleRedrawNeeded;
					model.PropertyChanged -= HandlePropertyChanged;
				}
				model = value;
				if ( model != null )
				{
					model.BatchCommitted += HandleRedrawNeeded;
					model.PropertyChanged += HandlePropertyChanged;
				}
				UpdateNativeControl();
			}
		}

		public override FrameworkElement Child
		{
			get { return child; }
			set
			{
				if ( child == value )
				{
					return;
				}
				child = value;
				UpdateNativeControl();
			}
		}

		public override void Dispose()
		{
			if ( !disposed )
			{
				disposed = true;
				Child = null;
				Model = default( TModel );
				Element = default( TElement );
			}
		}

		void ElementOnDoubleTap( object sender, MouseButtonEventArgs gestureEventArgs )
		{
			var view = Model as View;
			if ( view == null )
			{
				return;
			}
			foreach ( var current in view.GestureRecognizers.OfType<TapGestureRecognizer>().Where( g => g.NumberOfTapsRequired == 2 ) )
			{
				current.SendTapped( view );
				gestureEventArgs.Handled = true;
			}
		}

		void ElementOnTap( object sender, MouseButtonEventArgs gestureEventArgs )
		{
			var view = Model as View;
			if ( view == null )
			{
				return;
			}
			foreach ( var current in view.GestureRecognizers.OfType<TapGestureRecognizer>().Where( g => g.NumberOfTapsRequired == 1 ) )
			{
				current.SendTapped( view );
				gestureEventArgs.Handled = true;
			}
		}

		protected virtual void HandlePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == VisualElement.XProperty.PropertyName || e.PropertyName == VisualElement.YProperty.PropertyName || e.PropertyName == VisualElement.WidthProperty.PropertyName || e.PropertyName == VisualElement.HeightProperty.PropertyName || e.PropertyName == VisualElement.AnchorXProperty.PropertyName || e.PropertyName == VisualElement.AnchorYProperty.PropertyName || e.PropertyName == VisualElement.TranslationXProperty.PropertyName || e.PropertyName == VisualElement.TranslationYProperty.PropertyName || e.PropertyName == VisualElement.ScaleProperty.PropertyName || e.PropertyName == VisualElement.RotationProperty.PropertyName || e.PropertyName == VisualElement.RotationXProperty.PropertyName || e.PropertyName == VisualElement.RotationYProperty.PropertyName || e.PropertyName == VisualElement.IsVisibleProperty.PropertyName || e.PropertyName == VisualElement.OpacityProperty.PropertyName )
			{
				UpdateNativeControl();
			}
		}

		void HandleRedrawNeeded( object sender, EventArgs e )
		{
			UpdateNativeControl();
		}

		protected virtual void UpdateNativeControl()
		{
			if ( Model != null && Element != null )
			{
				VisualElement visualElement = model;
				element.Visibility = ( visualElement.IsVisible ? Visibility.Visible : Visibility.Collapsed );
				if ( Child != null )
				{
					Child.Visibility = visualElement.IsVisible ? Visibility.Visible : Visibility.Collapsed;
				}
				if ( visualElement.Batched || visualElement.Bounds.IsEmpty )
				{
					element.Opacity = 0.0;
					return;
				}
				element.Width = visualElement.Width;
				element.Height = visualElement.Height;
				if ( Child != null )
				{
					LayoutChild();
				}
				Canvas.SetLeft( element, visualElement.X + visualElement.TranslationX );
				Canvas.SetTop( element, visualElement.Y + visualElement.TranslationY );
				element.Opacity = visualElement.Opacity;
				element.RenderTransformOrigin = new System.Windows.Point( visualElement.AnchorX, visualElement.AnchorY );
				element.RenderTransform = new ScaleTransform
				{
					ScaleX = visualElement.Scale,
					ScaleY = visualElement.Scale
				};
				// TODO: http://blogs.msdn.com/b/greg_schechter/archive/2007/10/26/enter-the-planerator-dead-simple-3d-in-wpf-with-a-stupid-name.aspx
				/*element.Projection = new PlaneProjection
				{
					CenterOfRotationX = visualElement.AnchorX,
					CenterOfRotationY = visualElement.AnchorY,
					RotationX = -visualElement.RotationX,
					RotationY = -visualElement.RotationY,
					RotationZ = -visualElement.Rotation
				};*/
				OnUpdated();
				element.InvalidateArrange();
			}
		}

		protected virtual void LayoutChild()
		{
			var arg_1A_0 = Child;
			var tModel = Model;
			arg_1A_0.Width = tModel.Width;
			var arg_39_0 = Child;
			var tModel2 = Model;
			arg_39_0.Height = tModel2.Height;
		}
	}
}
