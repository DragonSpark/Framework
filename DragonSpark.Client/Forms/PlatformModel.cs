using System;
using DragonSpark.Activation.IoC.Commands;
using DragonSpark.Application.Forms;
using DragonSpark.Application.Forms.Rendering;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Mvvm;
using Xamarin.Forms;
using Size = System.Windows.Size;

namespace DragonSpark.Application.Client.Forms
{
	[RegisterAs( typeof(IPlatform) )]
	public class PlatformModel : BindableBase, IPlatform
	{
		public event EventHandler BindingContextChanged
		{
			add { Application.BindingContextChanged += value; }
			remove { Application.BindingContextChanged -= value; }
		}

		readonly IPlatformEngine engine;
		readonly global::Xamarin.Forms.Application application;

		public PlatformModel( IPlatformEngine engine, global::Xamarin.Forms.Application application )
		{
			this.engine = engine;
			this.application = application;
		}

		public Xamarin.Forms.Application Application
		{
			get { return application; }
		}

		public Size Size
		{
			get { return size; }
			set
			{
				if ( SetProperty( ref size, value ) )
				{
					Refresh();
				}
			}
		}	Size size;

		void Refresh()
		{
			Page.With( page => page.Layout( new Rectangle( 0.0, 0.0, Size.Width, Size.Height ) ) );
		}

		public object Content
		{

			get { return content; }
			private set 
			{
				if ( SetProperty( ref content, value ) )
				{
					Refresh();
				}
			}
		}	object content;

		void IPlatform.SetPage( Page current )
		{
			Page = current;
		}

		public Page Page
		{
			get { return page; }
			private set
			{
				if ( SetProperty( ref page, value ) )
				{
					page.Assign( this );
				
					Content = page.DetermineRenderer();
					// UpdateToolbarTracker();
				}
			}
		}	Page page;

		object IPlatform.BindingContext
		{
			get { return application.BindingContext; }
			set
			{
				if ( application.BindingContext != value )
				{
					application.BindingContext = value;
				}
			}
		}

		public IPlatformEngine Engine
		{
			get { return engine; }
		}
	}
}