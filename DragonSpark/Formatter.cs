using DragonSpark.Activation;
using DragonSpark.Coercion;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Composition;
using DragonSpark.Sources.Scopes;

namespace DragonSpark
{
	public interface IFormatter : IParameterizedSource<FormatterParameter, string>, IParameterizedSource<object, string> {}

	public sealed class Formatter : DecoratedParameterizedSource<FormatterParameter, string>, IFormatter
	{
		[Export]
		public static IFormatter Default { get; } = new Formatter();
		Formatter() : this( DefaultImplementation.Implementation.Apply( ConstructCoercer<FormatterParameter>.Default ).ToCache(), DefaultImplementation.Implementation ) {}

		readonly IParameterizedSource<object, string> general;

		[UsedImplicitly]
		public Formatter( IParameterizedSource<object, string> general, IParameterizedSource<FormatterParameter, string> source ) : base( source )
		{
			this.general = general;
		}

		public string Get( object parameter ) => general.Get( parameter );

		sealed class DefaultImplementation : ParameterizedSourceBase<FormatterParameter, string>
		{
			readonly static Func<FormatterParameter, string> Coerce = p => StringCoercer.Default.Coerce( p.Instance );

			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : this( FormattableSource.Default.Get ) {}

			readonly Func<object, IFormattable> factory;

			DefaultImplementation( Func<object, IFormattable> factory )
			{
				this.factory = factory;
			}

			public override string Get( FormatterParameter parameter ) => factory( parameter.Instance )?.ToString( parameter.Format, parameter.Provider ) ?? Coerce( parameter );
		}
	}

	public sealed class FormattableSource : DelegatedParameterizedSource<object, IFormattable>
	{
		public static FormattableSource Default { get; } = new FormattableSource();
		FormattableSource() : base( ConstructFromKnownTypes<IFormattable>.Default.GetValueDelegate().Cache() ) {}
	}
}