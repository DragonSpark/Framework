using DragonSpark.Composition;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using JetBrains.Annotations;
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
				var result = new ParameterizedScope<Type, object>( new Source( parameter ).Global )
					.Apply( ContainsSingletonPropertySpecification.Default )
					.Apply( Conventions )
					.ToSourceDelegate();
				return result;
			}

			sealed class Source : ParameterizedSourceBase<Type, object>
			{
				readonly static Func<Type, IParameterizedSource<object>> AccountedSource = SourceAccountedValues.Defaults.Get;
				readonly Func<Type, Func<object>> source;
				readonly Func<Type, IParameterizedSource<object>> accountedSource;

				public Source( Func<Type, Func<object>> source ) : this( source, AccountedSource ) {}

				[UsedImplicitly]
				public Source( Func<Type, Func<object>> source, Func<Type, IParameterizedSource<object>> accountedSource )
				{
					this.source = source;
					this.accountedSource = accountedSource;
				}

				public override object Get( Type parameter )
				{
					var invoke = source( parameter )?.Invoke();
					var result = invoke != null ? accountedSource( parameter ).Get( invoke ) : null;
					return result;
				}
			}
		}
	}
}