using PostSharp.Aspects;

namespace DragonSpark.Aspects.Validation
{
	public sealed class AutoValidationValidationAspect : AutoValidationAspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var controller = args.Instance as IAutoValidationController;
			if ( controller != null && !controller.IsActive )
			{
				var parameter = args.Arguments[0];
				args.ReturnValue = controller.Handles( parameter ) || controller.Marked( parameter, args.GetReturnValue<bool>() );
			}
			else
			{
				args.Proceed();
			}
		}
	}
}