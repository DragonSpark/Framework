namespace DragonSpark.Sources.Coercion
{
	public sealed class StringCoercer : Coercer<string>
	{
		public new static StringCoercer Default { get; } = new StringCoercer();
		StringCoercer() {}

		protected override string Coerce( object parameter ) => parameter.ToString();
	}
}