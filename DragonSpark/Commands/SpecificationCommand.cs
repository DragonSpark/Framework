using DragonSpark.Specifications;
using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Commands
{
	public class SpecificationCommand<T> : CommandBase<T>
	{
		readonly ISpecification<T> specification;
		readonly Action<T> command;

		public SpecificationCommand( ISpecification<T> specification, Action<T> command )
		{
			this.specification = specification;
			this.command = command;
		}

		public override void Execute( [Optional]T parameter )
		{
			var isSatisfiedBy = specification.IsSatisfiedBy( parameter );
			if ( isSatisfiedBy )
			{
				command( parameter );
			}
		}
	}
}