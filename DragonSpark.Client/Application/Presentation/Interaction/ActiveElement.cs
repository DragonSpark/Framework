using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ActiveElement : Behavior<ButtonBase>
	{
		bool clicked;

		protected override void OnAttached()
		{
			AssociatedObject.Click += AssociatedObject_Click;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Click -= AssociatedObject_Click;
		}

		void AssociatedObject_Click(object sender, RoutedEventArgs e)
		{
			clicked = true;
		}

		public bool IsActive
		{
			get { return GetValue( IsActiveProperty ).To<bool>(); }
			set { SetValue( IsActiveProperty, value ); }
		}	public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register( "IsActive", typeof(bool), typeof(ActiveElement), new PropertyMetadata( OnIsActiveChanged ) );

		static void OnIsActiveChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.To<ActiveElement>().UpdateActive();
		}

		void UpdateActive()
		{
			if ( clicked && !IsActive )
			{
				clicked = false;
				if ( Target != null )
				{
					Target.Focus();
				}
			}
		}

		Control Target
		{
			get { return target ?? ( target = AssociatedObject.FindName<Control>( TargetName ) ); }
		}	Control target;

		public string TargetName
		{
			get { return GetValue( TargetNameProperty ).To<string>(); }
			set { SetValue( TargetNameProperty, value ); }
		}	public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register( "TargetName", typeof(string), typeof(ActiveElement), null );
	}
}