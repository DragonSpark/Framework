using System;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.TypeSystem
{
	public abstract class SurrogateAttribute : Attribute, ISurrogate
	{
		protected SurrogateAttribute( [Required]Type @for )
		{
			For = @for;
		}

		public Type For { get; }
	}
}