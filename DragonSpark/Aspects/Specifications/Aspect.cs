using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace DragonSpark.Aspects.Specifications
{
	[LinesOfCodeAvoided( 1 ), ProvideAspectRole( KnownRoles.ParameterValidation ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Validation )]
	public sealed class Aspect : AspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var specification = args.Instance as ISpecification;
			if ( specification != null )
			{
				args.ReturnValue = specification.IsSatisfiedBy( args.Arguments[0] );
			}
			else
			{
				args.Proceed();
			}
		}
	}
}