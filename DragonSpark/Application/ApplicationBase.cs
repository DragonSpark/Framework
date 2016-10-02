using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using System.Linq;
using System.Windows.Input;

namespace DragonSpark.Application
{
	[ApplyAutoValidation]
	public abstract class ApplicationBase<T> : CompositeCommand<T>, IApplication<T>
	{
		readonly ISpecification<T> specification;
		protected ApplicationBase() : this( Items<ICommand>.Default ) {}

		protected ApplicationBase( params ICommand[] commands ) : this( new OnlyOnceSpecification<T>(), commands ) {}

		protected ApplicationBase( ISpecification<T> specification, params ICommand[] commands ) : base( commands.Distinct().Prioritize().Fixed() )
		{
			this.specification = specification;
		}

		public override bool IsSatisfiedBy( T parameter ) => specification.IsSatisfiedBy( parameter );
	}
}