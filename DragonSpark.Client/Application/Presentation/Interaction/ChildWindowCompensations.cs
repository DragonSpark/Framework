using System;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ChildWindowCompensations : Behavior<ChildWindow>
	{
		protected override void OnAttached()
		{
			AssociatedObject.Opened += AssociatedObject_Opened;
			AssociatedObject.Closing += AssociatedObjectClosing;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Opened -= AssociatedObject_Opened;
			AssociatedObject.Closing -= AssociatedObjectClosing;
			base.OnDetaching();
		}

		void AssociatedObjectClosing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			IsOpened = false;
		}

		public bool IsOpened
		{
			get { return GetValue( IsOpenedProperty ).To<bool>(); }
			set { SetValue( IsOpenedProperty, value ); }
		}	public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register( "IsOpened", typeof(bool), typeof(ChildWindowCompensations), null );
		
		void AssociatedObject_Opened( object sender, EventArgs e )
		{
			IsOpened = true;
		}
	}
}