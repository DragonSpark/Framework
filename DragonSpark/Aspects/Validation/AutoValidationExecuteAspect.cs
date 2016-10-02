using PostSharp.Aspects;
using System;

namespace DragonSpark.Aspects.Validation
{
	public sealed class AutoValidationExecuteAspect : AutoValidationAspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var controller = args.Instance as IAutoValidationController;
			if ( controller != null && !controller.IsActive )
			{
				args.ReturnValue = controller.Execute( args.Arguments[0], new Invocation( args.GetReturnValue ) ) ?? args.ReturnValue;
			}
			else
			{
				args.Proceed();
			}
		}

		sealed class Invocation : IInvocation
		{
			readonly Func<object> factory;

			public Invocation( Func<object> factory )
			{
				this.factory = factory;
			}

			public object Invoke( object parameter ) => factory();
		}
	}
}