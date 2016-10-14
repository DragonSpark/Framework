using DragonSpark.Composition;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Composition;

namespace DragonSpark.Activation.Location
{
	public sealed class SingletonLocator : ActivatorBase, ISingletonLocator
	{
		[Export]
		public static ISingletonLocator Default { get; } = new SingletonLocator();
		SingletonLocator() : this( SingletonDelegates.Default.Get ) {}

		public SingletonLocator( Func<Type, Func<object>> source ) : this( SourceFactory.Instance.Get( source ) ) {}

		SingletonLocator( Func<Type, object> source ) : base( new DelegatedAssignedSpecification<Type, object>( source ), source ) {}

		sealed class SourceFactory : ParameterizedSourceBase<Func<Type, Func<object>>, Func<Type, object>>
		{
			readonly static Alter<Type> Conventions = ConventionTypeSelector.Default.ToDelegate();

			public static SourceFactory Instance { get; } = new SourceFactory();
			SourceFactory() {}

			public override Func<Type, object> Get( Func<Type, Func<object>> parameter )
			{
				var source = new SpecificationParameterizedSource<Type, object>( Specification.DefaultNested, new Source( parameter ) ).ToSourceDelegate();
				var altered = new AlteredParameterizedSource<Type, object>( Conventions, source );
				var result = altered.ToSourceDelegate();
				return result;
			}

			sealed class Specification : AllSpecification<Type>
			{
				public static ISpecification<Type> DefaultNested { get; } = new Specification().ToCachedSpecification();
				Specification() : base( Common<Type>.Assigned, ContainsSingletonPropertySpecification.Default ) {}
			}

			sealed class Source : FactoryCache<Type, object>
			{
				readonly Func<Type, Func<object>> source;

				public Source( Func<Type, Func<object>> source )
				{
					this.source = source;
				}

				protected override object Create( Type parameter ) => source( parameter )?.Invoke();
			}
		}
	}
}