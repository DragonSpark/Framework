namespace DragonSpark.Coercion
{
	public sealed class StringCoercer : Coercer<string>
	{
		public new static StringCoercer Default { get; } = new StringCoercer();
		StringCoercer() {}

		protected override string Apply( object parameter ) => parameter.ToString();
	}
}