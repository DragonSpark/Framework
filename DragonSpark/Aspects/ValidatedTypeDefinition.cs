using DragonSpark.Aspects.Build;
using System;

namespace DragonSpark.Aspects
{
	public class ValidatedTypeDefinition : TypeDefinition, IValidatedTypeDefinition
	{
		public ValidatedTypeDefinition( Type declaringType, string execution ) : this( new MethodStore( declaringType, execution ) ) {}
		public ValidatedTypeDefinition( IMethodStore execution ) : this( execution.DeclaringType, execution ) {}
		public ValidatedTypeDefinition( Type declaringType, IMethodStore execution ) : this( declaringType, GenericSpecificationTypeDefinition.Default.Method, execution ) {}
		public ValidatedTypeDefinition( Type declaringType, string validation, string execution ) : this( declaringType, new MethodStore( declaringType, validation ), new MethodStore( declaringType, execution ) ) {}
		public ValidatedTypeDefinition( Type declaringType, IMethodStore validation, IMethodStore execution ) : base( declaringType, validation, execution )
		{
			Validation = validation;
			Execution = execution;
		}

		public IMethodStore Validation { get; }
		public IMethodStore Execution { get; }
	}
}