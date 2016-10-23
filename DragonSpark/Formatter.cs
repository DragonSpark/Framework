using DragonSpark.Activation;
using DragonSpark.Coercion;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Composition;

namespace DragonSpark
{
	public interface IFormatter : IParameterizedSource<FormatterParameter, string>, IParameterizedSource<object, string> {}

	public sealed class Formatter : DecoratedParameterizedSource<FormatterParameter, string>, IFormatter
	{
		[Export]
		public static IFormatter Default { get; } = new Formatter();
		Formatter() : this( Implementation.DefaultNested.Apply( ConstructCoercer<FormatterParameter>.Default ).ToCache(), Implementation.DefaultNested ) {}

		readonly IParameterizedSource<object, string> general;
		
		Formatter( IParameterizedSource<object, string> general, IParameterizedSource<FormatterParameter, string> source ) : base( source )
		{
			this.general = general;
		}

		public string Get( object parameter ) => general.Get( parameter );


		sealed class Implementation : ParameterizedSourceBase<FormatterParameter, string>
		{
			readonly static Func<FormatterParameter, string> Coerce = p => StringCoercer.Default.Coerce( p.Instance );

			public static Implementation DefaultNested { get; } = new Implementation();
			Implementation() : this( FormattableSource.Default.Get ) {}


			readonly Func<object, IFormattable> factory;

			Implementation( Func<object, IFormattable> factory )
			{
				this.factory = factory;
			}

			public override string Get( FormatterParameter parameter ) => factory( parameter.Instance )?.ToString( parameter.Format, parameter.Provider ) ?? Coerce( parameter );
		}
	}

	public sealed class FormattableSource : DelegatedParameterizedSource<object, IFormattable>
	{
		public static FormattableSource Default { get; } = new FormattableSource();
		FormattableSource() : base( ConstructFromKnownTypes<IFormattable>.Default.GetCurrentDelegate().Cache() ) {}
	}
}