using DragonSpark.Application.Client.Commanding;
using DragonSpark.Application.Client.Presentation;
using DragonSpark.Extensions;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Application.Client.Interaction
{
	public class BindingHost : System.Windows.Interactivity.Behavior<FrameworkElement>
	{
		readonly BindingListener listener = new BindingListener();

		protected override void OnAttached() 
		{
			base.OnAttached();

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;			
		}

		void AssociatedObjectOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			listener.Binding.With( binding => binding.ElementName.NullIfEmpty().Null( () => binding.Source = AssociatedObject ) );
			listener.Element = AssociatedObject;
			Refresh();
		}

		void AssociatedObjectOnUnloaded( object sender, RoutedEventArgs routedEventArgs )
		{
			listener.Clear();
		}
		
		void Refresh()
		{
			AssociatedObject.With( element => listener.Value = Item );
		}

		protected override void OnDetaching() {
			base.OnDetaching();

			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;			
		}
		public object Item
		{
			get { return GetValue( ItemProperty ).To<object>(); }
			set { SetValue( ItemProperty, value ); }
		}	public static readonly DependencyProperty ItemProperty = DependencyProperty.Register( "Item", typeof(object), typeof(BindingHost), new PropertyMetadata( OnItemChanged ) );

		public Binding ApplyTo
		{
			get { return listener.Binding; }
			set { listener.Binding = value; }
		}

		static void OnItemChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<BindingHost>( host => host.Refresh() );
		}
	}

	public class ApplyDialogResultCommand : ApplyNotificationResultCommand<IConfirmation, bool>
	{
		protected override void Assign( IConfirmation notification )
		{
			notification.Confirmed = Result;
		}

		protected override void Close( Window parameter )
		{
			parameter.DialogResult = Result;
		}
	}

	public class ApplySelectionResultCommand : ApplyNotificationResultCommand<OptionsNotification, string>
	{
		public ApplySelectionResultCommand()
		{}

		protected override void Assign( OptionsNotification notification )
		{
			notification.Result = Result;
		}
	}

	public abstract class ApplyNotificationResultCommand<TNotification, TResult> : CommandBase<Window> where TNotification : class
	{
		public TResult Result { get; set; }

		protected override void Execute( Window parameter )
		{
			parameter.DataContext.As<TNotification>( Assign );
			Close( parameter );
		}

		protected virtual void Close( Window parameter )
		{
			parameter.Close();
		}

		protected abstract void Assign( TNotification notification );
	}

	public class Notification : ViewObject, INotification
	{
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}   string title;

		public object Content
		{
			get { return content; }
			set { SetProperty(ref content, value); }
		}   object content;
	}

	public class OptionsNotification : SystemNotification<string>
	{
		public string Destruction
		{
			get { return destruction; }
			set { SetProperty(ref destruction, value); }
		}   string destruction;
	}

	public class DialogNotification : SystemNotification<bool?>, IConfirmation
	{
		public string Accept
		{
			get { return accept; }
			set { SetProperty(ref accept, value); }
		}   string accept;

		bool IConfirmation.Confirmed
		{
			get { return Result.GetValueOrDefault(); }
			set { Result = value; }
		}
	}

	public abstract class SystemNotification<TResult> : Notification
	{
		public string Cancel
		{
			get { return cancel; }
			set { SetProperty(ref cancel, value); }
		}   string cancel;

		public TResult Result
		{
			get { return result; }
			set { SetProperty(ref result, value); }
		}   TResult result;
	}

	[ContentProperty( "WindowStyle" )]
	public class PopupWindowAction : Microsoft.Practices.Prism.Interactivity.PopupWindowAction
	{
		public Style WindowStyle
		{
			get { return GetValue( WindowStyleProperty ).To<Style>(); }
			set { SetValue( WindowStyleProperty, value ); }
		}	public static readonly DependencyProperty WindowStyleProperty = DependencyProperty.Register( "WindowStyle", typeof(Style), typeof(PopupWindowAction), null );

		public Type WindowType
		{
			get { return GetValue( WindowTypeProperty ).To<Type>(); }
			set { SetValue( WindowTypeProperty, value ); }
		}	public static readonly DependencyProperty WindowTypeProperty = DependencyProperty.Register( "WindowType", typeof(Type), typeof(PopupWindowAction), null );
		
		protected override Window GetWindow( INotification notification )
		{
			var result = WindowType != null ? Create( notification ) : base.GetWindow( notification );
			return result;
		}

		Window Create( INotification notification )
		{
			var result = WindowType.Transform( Activator.CreateInstance<Window> ) ?? new Window();
			result.Style = WindowStyle ?? result.Style;
			result.Title = notification.Title;
			result.DataContext = result.Content = notification;
			PrepareContentForWindow( notification, result );
			return result;
		}
	}
}