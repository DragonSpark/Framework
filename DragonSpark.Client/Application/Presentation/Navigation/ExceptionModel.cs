using System;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Navigation
{
	[Singleton]
	public class ExceptionModel : ViewObject
	{
		internal void Update( Exception error, Uri requested, Uri handled )
		{
			Exception = error;
			Location = requested;
			Redirect = handled;
		}

		public Uri Location
		{
			get { return location; }
			private set { SetProperty( ref location, value, () => Location ); }
		}	Uri location;

		public Uri Redirect
		{
			get { return redirect; }
			private set { SetProperty( ref redirect, value, () => Redirect ); }
		}	Uri redirect;

	    public Exception Exception
	    {
	        get { return exception; }
	        private set { SetProperty( ref exception, value, () => Exception ); }
	    }   Exception exception;

	}
}