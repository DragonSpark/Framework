using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace DragonSpark.Aspects.Alteration
{
	[ProvideAspectRole( KnownRoles.ValueConversion ), LinesOfCodeAvoided( 1 ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation )]
	public sealed class Aspect : AspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var coercer = args.Instance as IAlteration;
			if ( coercer != null )
			{
				var arguments = args.Arguments;
				arguments.SetArgument( 0, coercer.Alter( arguments.GetArgument( 0 ) ) );
			}
			args.Proceed();
		}
	}
}