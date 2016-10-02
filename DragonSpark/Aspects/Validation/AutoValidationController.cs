using DragonSpark.Sources.Parameterized;
using PostSharp.Aspects;
using System.Threading;

namespace DragonSpark.Aspects.Validation
{
	sealed class AutoValidationController : ThreadLocal<object>, IAutoValidationController, IAspectHub
	{
		readonly static object Active = new object();
		readonly IParameterValidationAdapter validator;
		
		public AutoValidationController( IParameterValidationAdapter validator )
		{
			this.validator = validator;
		}

		public bool IsActive => Value == Active;

		public bool Handles( object parameter ) => Handler?.Handles( parameter ) ?? false;

		public void MarkValid( object parameter, bool valid ) => Value = valid ? parameter : null;

		object Clear( object result = null )
		{
			Value = null;
			return result;
		}

		public object Execute( object parameter, IInvocation proceed )
		{
			object handled = null;
			if ( Handler?.Handle( parameter, out handled ) ?? false )
			{
				return Clear( handled );
			}
			
			var marked = Value != null && Equals( Value, parameter );
			Value = Active;
			var result = marked || validator.IsSatisfiedBy( parameter ) ? proceed.Invoke( parameter ) : null;
			return Clear( result );
		}

		IParameterAwareHandler Handler { get; set; }

		public void Register( IAspect aspect )
		{
			var methodAware = aspect as IMethodAware;
			if ( methodAware != null && validator.IsSatisfiedBy( methodAware.Method ) )
			{
				var handler = aspect as IParameterAwareHandler;
				if ( handler != null )
				{
					Handler = Handler != null ? new LinkedParameterAwareHandler( handler, Handler ) : handler;
				}
			}
		}
	}
}