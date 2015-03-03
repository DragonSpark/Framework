﻿
using DragonSpark.Application.Client.Eventing;
using DragonSpark.Application.Client.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Client.Forms.Interaction
{
	public class CloseOnReturningBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;
		}

		void AssociatedObjectOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			this.Event<ReturningEvent>().Subscribe( OnReturning );
		}

		void OnReturning( CancelEventArgs eventArgs )
		{
			eventArgs.Cancel = true;
			var window = AssociatedObject.GetParentOfType<Window>();
			window.Close();
		}
		
		void AssociatedObjectOnUnloaded( object sender, RoutedEventArgs routedEventArgs )
		{
			this.Event<ReturningEvent>().Unsubscribe( OnReturning );
		}
	}
}
