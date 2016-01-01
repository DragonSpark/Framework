using System;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class ActivationParameter
	{
		protected ActivationParameter( [Required]Type type )
		{
			Type = type;
		}

		public Type Type { get; }

	}
}