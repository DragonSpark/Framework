using DragonSpark.Aspects;
using DragonSpark.Aspects.Validation;
using DragonSpark.Extensions;
using System;

namespace DragonSpark.Sources.Parameterized
{
	abstract class AutoValidatingSourceBase<TParameter, TResult>
	{
		readonly IAutoValidationController controller;
		readonly Func<TParameter, bool> specification;
		readonly IInvocation invocation;

		protected AutoValidatingSourceBase( IAutoValidationController controller, Func<TParameter, bool> specification, Func<TParameter, TResult> source ) : this( controller, specification, new DelegatedInvocation<TParameter, TResult>( source ) ) {}

		AutoValidatingSourceBase( IAutoValidationController controller, Func<TParameter, bool> specification, IInvocation invocation )
		{
			this.controller = controller;
			this.specification = specification;
			this.invocation = invocation;
		}

		public bool IsSatisfiedBy( TParameter parameter ) => controller.Handles( parameter ) || controller.Marked( parameter, specification( parameter ) );

		public TResult Get( TParameter parameter ) => controller.Execute( parameter, invocation ).As<TResult>();
	}
}