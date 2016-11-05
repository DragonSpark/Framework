using DragonSpark.Composition;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
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

		SingletonLocator( Func<Type, object> second ) : base( new DelegatedAssignedSpecification<Type, object>( second ), second ) {}

		sealed class SourceFactory : ParameterizedSourceBase<Func<Type, Func<object>>, Func<Type, object>>
		{
			readonly static Alter<Type> Conventions = ConventionTypeSelector.Default.ToDelegate();

			public static SourceFactory Instance { get; } = new SourceFactory();
			SourceFactory() {}

			public override Func<Type, object> Get( Func<Type, Func<object>> parameter )
			{
				var result = new Source( parameter ).ToSingletonScope()
					.Apply( ContainsSingletonPropertySpecification.Default )
					.Apply( Conventions )
					.ToDelegate();
				return result;
			}

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
}