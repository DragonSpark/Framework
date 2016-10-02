using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;

namespace DragonSpark.Aspects.Validation
{
	[IntroduceInterface( typeof(IAutoValidationController) )]
	[LinesOfCodeAvoided( 4 ), ProvideAspectRole( KnownRoles.EnhancedValidation ), 
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Validation ),
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, KnownRoles.ParameterValidation )
		]
	public sealed class ApplyAutoValidationAttribute : ApplyInstanceAspectBase, IAutoValidationController
	{
		public ApplyAutoValidationAttribute() : base( Support.Default ) {}

		public override void RuntimeInitializeInstance() => Controller = Defaults.ControllerSource( Instance );
		IAutoValidationController Controller { get; set; }

		bool IAutoValidationController.IsActive => Controller.IsActive;
		bool IAutoValidationController.Handles( object parameter ) => Controller.Handles( parameter );
		void IAutoValidationController.MarkValid( object parameter, bool valid ) => Controller.MarkValid( parameter, valid );
		object IAutoValidationController.Execute( object parameter, IInvocation proceed ) => Controller.Execute( parameter, proceed );
	}
}