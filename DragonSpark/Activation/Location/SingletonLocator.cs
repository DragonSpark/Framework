using DragonSpark.Composition;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Composition;

namespace DragonSpark.Activation.Location
{
	public sealed class SingletonLocator : ActivatorBase, ISingletonLocator
	{
		readonly static ISpecification<Type> Specification = Common<Type>.Assigned.And( ContainsSingletonPropertySpecification.Default );
		
		[Export]
		public static ISingletonLocator Default { get; } = new SingletonLocator();
		SingletonLocator() : this( new Source( SingletonDelegates.Default.Get ).Apply( Specification ).Apply( ConventionTypeSelector.Default ).ToCache().ToSourceDelegate() ) {}

		public SingletonLocator( Func<Type, Func<object>> inner ) : this( new Source( inner ) ) {}

		SingletonLocator( IParameterizedSource<Type, object> source ) : this( source.Apply( ConventionTypeSelector.Default ).ToCache().ToSourceDelegate() ) {}
		SingletonLocator( Func<Type, object> source ) : base( new DelegatedAssignedSpecification<Type, object>( source ), source ) {}

		sealed class Source : ParameterizedSourceBase<Type, object>
		{
			readonly Func<Type, Func<object>> source;

			public Source( Func<Type, Func<object>> source )
			{
				this.source = source;
			}

			public override object Get( Type parameter ) => source( parameter )?.Invoke();
		}
	}
}