using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using System.Windows.Input;

namespace DragonSpark.Application.Setup
{
	[ApplyAutoValidation]
	public class DeclarativeSetup : CompositeCommand, ISetup// , ISpecificationAware<object>
	{
		public static ISetup Current() => AmbientStack.GetCurrentItem<ISetup>();

		readonly ISpecification<object> specification;

		public DeclarativeSetup() : this( Priority.Normal ) {}
		public DeclarativeSetup( Priority priority = Priority.Normal ) : this( priority, Items<ICommand>.Default ) {}
		public DeclarativeSetup( params ICommand[] commands ) : this( Priority.Normal, commands ) {}
		public DeclarativeSetup( Priority priority = Priority.Normal, params ICommand[] commands ) : this( new OncePerScopeSpecification<object>(), priority, commands ) {}
		public DeclarativeSetup( ISpecification<object> specification, Priority priority = Priority.Normal, params ICommand[] commands ) : base( commands )
		{
			this.specification = specification;
			Priority = priority;
		}

		public override bool IsSatisfiedBy( object parameter ) => specification.IsSatisfiedBy( parameter );

		public Priority Priority { get; set; }

		public DeclarativeCollection<object> Items { get; } = new DeclarativeCollection<object>();

		public override void Execute( object parameter )
		{
			using ( new AmbientStackCommand<ISetup>().Run( this ) )
			{
				base.Execute( parameter );
			}
		}
	}
}
