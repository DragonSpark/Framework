using DragonSpark.Application.Client.Commanding;
using DragonSpark.Application.Client.Presentation;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Markup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Application.Client.Interaction
{
	public class ApplyDialogResultCommand : CommandBase<Window>
	{
		public bool Result { get; set; }

		protected override void Execute( Window parameter )
		{
			parameter.DataContext.As<IConfirmation>( confirmation => confirmation.Confirmed = Result );
			parameter.DialogResult = Result;
		}
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

	public class DialogNotification : Notification, IConfirmation
	{
		public string Accept
		{
			get { return accept; }
			set { SetProperty(ref accept, value); }
		}   string accept;

		public string Cancel
		{
			get { return cancel; }
			set { SetProperty(ref cancel, value); }
		}   string cancel;

		public bool? Result
		{
			get { return result; }
			private set { SetProperty(ref result, value); }
		}   bool? result;

		bool IConfirmation.Confirmed
		{
			get { return Result.GetValueOrDefault(); }
			set { Result = value; }
		}
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
			var window = WindowType.Transform( Activator.CreateInstance<Window> ) ?? new Window();
			window.Style = WindowStyle ?? window.Style;
			window.Title = notification.Title;
			window.DataContext = window.Content = notification;
			PrepareContentForWindow( notification, window );
			return window;
		}
	}
}